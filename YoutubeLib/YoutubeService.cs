#define DEBUG
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using HtmlAgilityPack;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using YoutubeLib.Extensions;
using YoutubeLib.Playlists;
using YoutubeLib.Streams;
using YoutubeLib.Videos;
using YoutubeLib.Videos.Deciphering;
using HttpClient = YoutubeLib.Http.HttpClient;

namespace YoutubeLib
{
    /// <summary>
    ///     Represents the default YouTube service.
    /// </summary>
    public sealed class YoutubeService : IYoutubeService
    {
        /// <inheritdoc />
        public async Task DownloadMediaStreamAsync(StreamBase mediaStream, string filePath)
        {
            await HttpClient.DownloadToFileAsync(mediaStream.Url, filePath);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<StreamBase>> GetMediaStreamsAsync(string url)
        {
            var videoId = Utils.ExtractVideoId(url);
            if (string.IsNullOrWhiteSpace(videoId))
            {
                return Enumerable.Empty<StreamBase>();
            }

            var videoInfo = await GetVideoInfoAsync(url).ConfigureAwait(false);
            var playerSourceCode = await GetPlayerSourceCode(url).ConfigureAwait(false);
            var mixedStreams = new List<MixedStream>();
            foreach (var encodedStreamInfo in videoInfo.GetValueOrDefault("url_encoded_fmt_stream_map", string.Empty)
                .Split(','))
            {
                var streamInfo = DecodeStreamDetails(encodedStreamInfo);
                var itag = int.Parse(streamInfo.GetValueOrDefault("itag", "0"));
                var streamUrl = streamInfo.GetValueOrDefault("url"); // Idek how this is possible because all streams (should) provide URLs
                if (streamUrl == null)
                {
                    continue;
                }

                var signature = streamInfo.GetValueOrDefault("s");
                if (!string.IsNullOrWhiteSpace(signature))
                {
                    // Decipher the signature
                    signature = playerSourceCode.ApplyDecipherOperations(signature);
                    //Utils.AddQueryParameters(url, new {signature}); This solution is not viable as the value of the 'sp' parameter may vary

                    // Add a signature parameter to the original stream URL
                    var signatureParameter = streamInfo.GetValueOrDefault("sp");
                    streamUrl +=
                        $"&{(string.IsNullOrWhiteSpace(signatureParameter) ? "signature" : signatureParameter)}={signature}";
                }

                var type = streamInfo["type"];
                var streamType = Utils.GetStreamType(type.Substring(0, type.IndexOf(';')));
                var (videoCodec, audioCodec) = Utils.GetCodecs(type.Substring(type.IndexOf(';') + 1));
                var quality = ItagDescriptor.KnownDescriptors.GetValueOrDefault(itag, ItagDescriptor.DefaultDescriptor)
                    .Quality;
                mixedStreams.Add(new MixedStream(itag, streamUrl, streamType, quality, audioCodec, videoCodec));
            }

            var adaptiveStreams = new List<AdaptiveStream>();
            foreach (var encodedStreamInfo in videoInfo.GetValueOrDefault("adaptive_fmts", string.Empty).Split(','))
            {
                var streamInfo = DecodeStreamDetails(encodedStreamInfo);
                var itag = int.Parse(streamInfo.GetValueOrDefault("itag", "0"));
                var streamUrl = streamInfo.GetValueOrDefault("url");
                if (streamUrl == null)
                {
                    continue;
                }

                var signature = streamInfo.GetValueOrDefault("s");
                if (!string.IsNullOrWhiteSpace(signature))
                {
                    signature = playerSourceCode.ApplyDecipherOperations(signature);
                    var signatureParameter = streamInfo.GetValueOrDefault("sp");
                    streamUrl +=
                        $"&{(string.IsNullOrWhiteSpace(signatureParameter) ? "signature" : signatureParameter)}={signature}";
                }

                var type = streamInfo["type"];
                var streamType = Utils.GetStreamType(type.Substring(0, type.IndexOf(';')));
                var (videoCodec, audioCodec) = Utils.GetCodecs(type.Substring(type.IndexOf(';') + 1));
                var clen = int.Parse(streamInfo["clen"]);
                var bitrate = int.Parse(streamInfo.GetValueOrDefault("bitrate", "0"));
                var frameRate = int.Parse(streamInfo.GetValueOrDefault("fps", "0"));
                var quality = ItagDescriptor.KnownDescriptors.GetValueOrDefault(itag, ItagDescriptor.DefaultDescriptor)
                    .Quality;
                adaptiveStreams.Add(new AdaptiveStream(itag, streamUrl, streamType, quality,
                    videoCodec == VideoCodec.Unknown ? AdaptiveType.AudioOnly : AdaptiveType.VideoOnly, clen, frameRate,
                    bitrate, quality.HasValue ? Utils.GetResolutionFromQuality(quality.Value) : new Resolution(0, 0)));
            }

            return mixedStreams.Cast<StreamBase>().Concat(adaptiveStreams);

            IDictionary<string, string> DecodeStreamDetails(string source)
            {
                var data = new Dictionary<string, string>();
                foreach (var property in source.Split('&'))
                {
                    var indexOfAssignment = property.IndexOf('=');
                    if (indexOfAssignment <= 0)
                    {
                        continue;
                    }

                    var key = property.Substring(0, indexOfAssignment);
                    var value = property.Substring(indexOfAssignment + 1, property.Length - key.Length - 1);
                    data[key] = HttpUtility.UrlDecode(value);
                }

                return data;
            }
        }

        /// <inheritdoc />
        public async Task DownloadMediaStreamAsync(StreamBase mediaStream, Stream outputStream)
        {
            using (var memoryStream = new MemoryStream())
            {
                var serializer = new BinaryFormatter();
                serializer.Serialize(memoryStream, mediaStream);
                await outputStream.WriteAsync(memoryStream.ToArray(), 0, (int) memoryStream.Length).ConfigureAwait(false);
            }
        }

        /// <inheritdoc />
        public async Task<Video> GetVideoAsync(string url)
        {
            // Get the video details
            var videoData = await GetVideoInfoAsync(url);
            var playerResponse = JObject.Parse(videoData["player_response"]);
            var videoDetails = playerResponse["videoDetails"];
            //var keywords = videoDetails["keywords"].ToObject<string[]>();
            var thumbnails = videoDetails["thumbnail"]["thumbnails"].ToObject<Thumbnail[]>();

            var video = videoDetails.ToObject<Video>();
            //video.Keywords = keywords;
            video.Thumbnails =
                thumbnails; // This seems to be the right approach when it comes to deserializing arrays from nested objects
            return video;
        }

        /// <inheritdoc />
        public async Task<Playlist> GetPlaylistAsync(string url)
        {
            var playlistId = Utils.ExtractPlaylistId(url);
            if (string.IsNullOrWhiteSpace(playlistId))
            {
                throw new ArgumentException("Invalid playlist.", nameof(url));
            }

            // This request returns up to 200 videos. In order to obtain information about the rest of the playlist we have to send multiple requests
            var response =
                await HttpClient.Instance.GetAsync(
                    $"https://www.youtube.com/list_ajax?style=json&action_get_list=1&list={playlistId}").ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var playlist = JsonConvert.DeserializeObject<Playlist>(HttpUtility.UrlDecode(content));
            var cachedIds = playlist.Videos.Select(v => v.EncryptedId).ToList();
            var index = 201;
            int numberOfNewVideos;
            do
            {
                numberOfNewVideos = 0;
                response = await HttpClient.Instance.GetAsync(
                    $"https://www.youtube.com/list_ajax?style=json&action_get_list=1&list={playlistId}&index={index}");
                var videos = JToken.Parse(await response.Content.ReadAsStringAsync().ConfigureAwait(false))["video"]
                    .ToObject<PlaylistItem[]>();
                foreach (var video in videos)
                {
                    if (cachedIds.Contains(video.EncryptedId))
                    {
                        continue;
                    }

                    cachedIds.Add(video.EncryptedId);
                    playlist.AddVideo(video);
                    ++numberOfNewVideos;
                }

                index += 100;
            } while (numberOfNewVideos > 0);

            return playlist;
        }

        private async Task<PlayerSourceCode> GetPlayerSourceCode(string url)
        {
            var videoId = Utils.ExtractVideoId(url);
            if (string.IsNullOrWhiteSpace(videoId))
            {
                throw new ArgumentException("Invalid video.", nameof(url));
            }

            var response = await HttpClient.Instance.GetAsync($"https://youtube.com/embed/{videoId}",
                HttpCompletionOption.ResponseContentRead).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            var embedPageHtml = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var sessionToken = Regex.Match(embedPageHtml, @"sts:\s*(\d+)").Groups[1].Value;
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(embedPageHtml);

            var playerSourceCodeRegex = new Regex("\"js\":\"(?<sourcelink>\\S[^,]+)\""); // Holy shit I'm bad at this
            var playerSourceLink = string.Empty;
            foreach (var scriptNode in htmlDocument.DocumentNode.Descendants("script"))
            {
                Match match;
                if ((match = playerSourceCodeRegex.Match(scriptNode.InnerHtml)).Success)
                {
                    playerSourceLink = match.Groups["sourcelink"].Value.Replace("\\", "").Trim('/');
                    break;
                }
            }

            Debug.Assert(playerSourceLink != null, "Player source link must not be null");

            // The player's source code link is relative, thus we have to prepend YouTube's host and then download the source code
            playerSourceLink = $"https://youtube.com/{playerSourceLink}";
            response = await HttpClient.Instance.GetAsync(playerSourceLink, HttpCompletionOption.ResponseContentRead).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            // Get the function that handles deciphering
            var deciphererFuncName = Regex.Match(content,
                @"(\w+)=function\(\w+\){(\w+)=\2\.split\(\x22{2}\);.*?return\s+\2\.join\(\x22{2}\)}").Groups[1].Value;
            var deciphererFuncBody = Regex.Match(content,
                    @"(?!h\.)" + Regex.Escape(deciphererFuncName) + @"=function\(\w+\)\{(.*?)\}",
                    RegexOptions.Singleline)
                .Groups[1].Value;
            var deciphererFuncBodyStatements = deciphererFuncBody.Split(';');
            var deciphererDefinitionName =
                Regex.Match(deciphererFuncBody, "(\\w+).\\w+\\(\\w+,\\d+\\);").Groups[1].Value;
            var deciphererDefinitionBody = Regex.Match(content,
                @"var\s+" +
                Regex.Escape(deciphererDefinitionName) +
                @"=\{(\w+:function\(\w+(,\w+)?\)\{(.*?)\}),?\};", RegexOptions.Singleline).Groups[0].Value;

            var decipherFunctions = new List<DecipherFunctionBase>();
            foreach (var statement in deciphererFuncBodyStatements)
            {
                var calledFuncName = Regex.Match(statement, @"\w+(?:.|\[)(\""?\w+(?:\"")?)\]?\(").Groups[1].Value;
                if (string.IsNullOrWhiteSpace(calledFuncName))
                {
                    continue;
                }

                if (Regex.IsMatch(deciphererDefinitionBody, $@"{Regex.Escape(calledFuncName)}:\bfunction\b\(\w+\)"))
                {
                    decipherFunctions.Add(new ReverseDecipherFunction());
                }

                if (Regex.IsMatch(deciphererDefinitionBody,
                    $@"{Regex.Escape(calledFuncName)}:\bfunction\b\([a],b\).(\breturn\b)?.?\w+\."))
                {
                    decipherFunctions.Add(
                        new SliceDecipherFunction(int.Parse(Regex.Match(statement, @"\(\w+,(\d+)\)").Groups[1].Value)));
                }

                if (Regex.IsMatch(deciphererDefinitionBody,
                    $@"{Regex.Escape(calledFuncName)}:\bfunction\b\(\w+\,\w\).\bvar\b.\bc=a\b"))
                {
                    decipherFunctions.Add(
                        new SwapDecipherFunction(int.Parse(Regex.Match(statement, @"\(\w+,(\d+)\)").Groups[1].Value)));
                }
            }

            return new PlayerSourceCode(sessionToken, decipherFunctions);
        }

        private async Task<IDictionary<string, string>> GetVideoInfoAsync(string url)
        {
            var videoId = Utils.ExtractVideoId(url);
            if (string.IsNullOrWhiteSpace(videoId))
            {
                throw new ArgumentException("Invalid video.", nameof(url));
            }

            var data = new Dictionary<string, string>();
            var response = await HttpClient.Instance.GetAsync(
                $"https://youtube.com/get_video_info?video_id={videoId}&el=detailpage&hl=en",
                HttpCompletionOption.ResponseContentRead).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            var encodedContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var responseData = encodedContent.Split('&');
            foreach (var line in responseData)
            {
                var indexOfAssignment = line.IndexOf('=');
                if (indexOfAssignment <= 0)
                {
                    continue;
                }

                var key = line.Substring(0, indexOfAssignment);
                var value = line.Substring(indexOfAssignment + 1, line.Length - key.Length - 1);
                data[key] = HttpUtility.UrlDecode(value);
            }

            return data;
        }
    }
}
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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using YoutubeLib.Extensions;
using YoutubeLib.Videos;
using HtmlAgilityPack;
using YoutubeLib.Channels;
using YoutubeLib.Streams;
using YoutubeLib.Videos.Deciphering;
using YoutubeLib.Extensions;

namespace YoutubeLib
{
    /// <summary>
    ///     Represents the default YouTube service.
    /// </summary>
    public sealed class YoutubeService : IYoutubeService
    {
        private static readonly Regex PlayerSourceCodeRegex = new Regex("\"js\":\"(?<sourcelink>\\S[^,]+)\""); // Holy shit I'm bad at this

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
                HttpCompletionOption.ResponseContentRead);
            response.EnsureSuccessStatusCode();
            var encodedContent = await response.Content.ReadAsStringAsync();
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

        /// <inheritdoc />
        public async Task<Channel> GetChannelAsync(string channelId)
        {
            return null;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<StreamBase>> GetMediaStreamsAsync(string url)
        {
            var videoId = Utils.ExtractVideoId(url);
            if (string.IsNullOrWhiteSpace(videoId))
            {
                return Enumerable.Empty<StreamBase>();
            }

            var videoInfo = await GetVideoInfoAsync(url);
            var playerSourceCode = await GetPlayerSourceCode(url);
            var mixedStreams = new List<MixedStream>();
            foreach (var encodedStreamInfo in videoInfo.GetValueOrDefault("url_encoded_fmt_stream_map", string.Empty).Split(','))
            {
                var streamInfo = DecodeStreamDetails(encodedStreamInfo);
                var itag = int.Parse(streamInfo["itag"]);
                var streamUrl = streamInfo["url"];
                var signature = streamInfo.GetValueOrDefault("s");
                if (!string.IsNullOrWhiteSpace(signature))
                {
                    // Decipher the signature
                    signature = playerSourceCode.ApplyDecihperOperations(signature);
                    //Utils.AddQueryParameters(url, new {signature}); This solution is not viable as the 'sp' parameter may vary

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
                var itag = int.Parse(streamInfo["itag"]);
                var streamUrl = streamInfo["url"];
                var signature = streamInfo.GetValueOrDefault("s");
                if (!string.IsNullOrWhiteSpace(signature))
                {
                    signature = playerSourceCode.ApplyDecihperOperations(signature);
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
                    bitrate, quality.HasValue ? Utils.GetResolutionFromQuality(quality.Value) : new Resolution(1, 1)));
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
                outputStream.Write(memoryStream.ToArray(), 0, (int) memoryStream.Length);
            }
        }

        /// <inheritdoc />
        public async Task DownloadMediaStreamAsync(StreamBase mediaStream, string filePath)
        {
            await HttpClient.DownloadToFileAsync(mediaStream.Url, filePath);
        }

        private async Task<PlayerSourceCode> GetPlayerSourceCode(string url)
        {
            var videoId = Utils.ExtractVideoId(url);
            if (string.IsNullOrWhiteSpace(videoId))
            {
                throw new ArgumentException("Invalid video.", nameof(url));
            }

            var response = await HttpClient.Instance.GetAsync($"https://youtube.com/embed/{videoId}", HttpCompletionOption.ResponseContentRead);
            response.EnsureSuccessStatusCode();
            var embedPageHtml = await response.Content.ReadAsStringAsync();
            var sessionToken = Regex.Match(embedPageHtml, @"sts:\s*(\d+)").Groups[1].Value;
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(embedPageHtml);

            var playerSourceLink = string.Empty;
            foreach (var scriptNode in htmlDocument.DocumentNode.Descendants("script"))
            {
                Match match;
                if ((match = PlayerSourceCodeRegex.Match(scriptNode.InnerHtml)).Success)
                {
                    playerSourceLink = match.Groups["sourcelink"].Value.Replace("\\", "").Trim('/');
                    break;
                }
            }
            
            // The player's source code link is relative, thus we have to prepend YouTube's host and then download the source code
            playerSourceLink = $"https://youtube.com/{playerSourceLink}";
            response = await HttpClient.Instance.GetAsync(playerSourceLink, HttpCompletionOption.ResponseContentRead);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();

            // Get the function that handles deciphering
            var deciphererFuncName = Regex.Match(content,
               @"(\w+)=function\(\w+\){(\w+)=\2\.split\(\x22{2}\);.*?return\s+\2\.join\(\x22{2}\)}").Groups[1].Value;
            var deciphererFuncBody = Regex.Match(content,
                @"(?!h\.)" + Regex.Escape(deciphererFuncName) + @"=function\(\w+\)\{(.*?)\}", RegexOptions.Singleline).Groups[1].Value;
            var deciphererFuncBodyStatements = deciphererFuncBody.Split(';');
            var deciphererDefinitionName = Regex.Match(deciphererFuncBody, "(\\w+).\\w+\\(\\w+,\\d+\\);").Groups[1].Value;
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

                if (Regex.IsMatch(deciphererDefinitionBody, $@"{Regex.Escape(calledFuncName)}:\bfunction\b\([a],b\).(\breturn\b)?.?\w+\."))
                {
                    decipherFunctions.Add(new SliceDecipherFunction(int.Parse(Regex.Match(statement, @"\(\w+,(\d+)\)").Groups[1].Value)));
                }

                if (Regex.IsMatch(deciphererDefinitionBody, $@"{Regex.Escape(calledFuncName)}:\bfunction\b\(\w+\,\w\).\bvar\b.\bc=a\b"))
                {
                    decipherFunctions.Add(new SwapDecipherFunction(int.Parse(Regex.Match(statement, @"\(\w+,(\d+)\)").Groups[1].Value)));
                }
            }

            return new PlayerSourceCode(sessionToken, decipherFunctions);
        }

        /// <inheritdoc />
        public async Task<Video> GetVideoAsync(string url)
        {
            // Get the video details
            var videoData = await GetVideoInfoAsync(url);
            var playerResponse = JObject.Parse(videoData["player_response"]);
            var videoDetails = playerResponse["videoDetails"];
            var keywords = videoDetails["keywords"].ToObject<string[]>();
            var thumbnails = videoDetails["thumbnail"]["thumbnails"].ToObject<Thumbnail[]>();

            var video = videoDetails.ToObject<Video>();
            video.Keywords = keywords;
            video.Thumbnails = thumbnails; // TODO: This is a temp fix. Figure out a way to deserialize thumbnail data from the videoDetails JSON
            return video;
        }
    }
}
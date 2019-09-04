using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using YoutubeLib.Streams;
using YoutubeLib.Videos;

namespace YoutubeLib
{
    /// <summary>
    ///     Exposes utility methods.
    /// </summary>
    public static class Utils
    {
        private static readonly Regex VideoIdRegex =
            new Regex(
                @"(?:https?:\/\/)?(?:www\.)?youtu(?:(\.be)|(be\.com\/(?:(watch)|(v\/))))(?(1)\/(?<videoId>.*)|(?(3)\?v=(?<videoId>.*)|(?<videoId>.*)))");

        /// <summary>
        ///     Updates the specified URL with the specified parameters.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="params">An enumerable collection of anonymous types that represent the parameters.</param>
        /// <returns>The updated URL.</returns>
        public static string AddQueryParameters(string url, params object[] @params)
        {
            IEnumerable<(string, object)> ResolveAnonymousType(object obj)
            {
                var type = obj.GetType();
                if (type.Name.Contains("AnonymousType") ||
                    type.GetCustomAttributes(typeof(CompilerGeneratedAttribute), false).Length == 0)
                {
                    return Enumerable.Empty<(string, object)>();
                }

                return type.GetProperties().Select(p => (p.Name, p.GetValue(obj, null)));
            }

            var stringBuilder = new StringBuilder(url);
            foreach (var param in @params)
            {
                foreach (var (paramName, paramValue) in ResolveAnonymousType(param))
                {
                    stringBuilder.Append($"&{paramName}={paramValue}");
                }
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        ///     Extracts a YouTube playlist ID based on the given input string.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <returns>The playlist ID.</returns>
        public static string ExtractPlaylistId(string input)
        {
            return HttpUtility.ParseQueryString(input)["list"];
        }

        // TODO: Support various URL authorities (youtu.be, youtube.com/v/ID etc)

        /// <summary>
        ///     Extracts a youtube video ID based on the given input string.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <returns>The video ID.</returns>
        public static string ExtractVideoId(string input)
        {
            //return HttpUtility.ParseQueryString(new Uri(input).Query)["v"];
            var match = VideoIdRegex.Match(input ?? string.Empty);
            if (!match.Success)
            {
                return string.Empty;
            }

            var videoId = match.Groups["videoId"];
            return videoId == null ? string.Empty : videoId.Value;
        }

        /// <summary>
        ///     Parses the video and audio codecs from the specified input string.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <returns>A <see cref="(T1, T2)" /> representing the parsed video and audio codecs.</returns>
        public static (VideoCodec, AudioCodec) GetCodecs(string input)
        {
            var codecs = input.Split(',');
            if (codecs.Length == 0)
            {
                return (VideoCodec.Unknown, AudioCodec.Unknown);
            }

            var audioCodec = AudioCodec.Unknown;
            if (!Enum.TryParse(codecs[0], true, out VideoCodec videoCodec))
            {
                videoCodec = VideoCodec.Unknown;
                Enum.TryParse(codecs[0], true, out audioCodec);
            }

            if (codecs.Length != 2)
            {
                return (videoCodec, audioCodec);
            }

            if (!Enum.TryParse(codecs.ElementAtOrDefault(1), true, out audioCodec))
            {
                audioCodec = AudioCodec.Unknown;
            }

            return (videoCodec, audioCodec);
        }

        /// <summary>
        ///     Returns the resolution inferred from the specified video quality.
        /// </summary>
        /// <param name="videoQuality">The video quality.</param>
        /// <returns>The resolution for the specified video quality.</returns>
        public static Resolution GetResolutionFromQuality(VideoQuality videoQuality)
        {
            switch (videoQuality)
            {
                case VideoQuality.Low144:
                    return new Resolution(256, 144);
                case VideoQuality.Low240:
                    return new Resolution(426, 240);
                case VideoQuality.Medium360:
                    return new Resolution(640, 360);
                case VideoQuality.Medium480:
                    return new Resolution(854, 480);
                case VideoQuality.High720:
                    return new Resolution(1280, 720);
                case VideoQuality.High1080:
                    return new Resolution(1920, 1080);
                case VideoQuality.High1440:
                    return new Resolution(2560, 1440);
                case VideoQuality.High2160:
                    return new Resolution(3840, 2160);
                case VideoQuality.High3072:
                    return new Resolution(5462, 3072);
                case VideoQuality.High4320:
                    return new Resolution(7680, 4320);
                default:
                    throw new ArgumentOutOfRangeException(nameof(videoQuality), videoQuality, null);
            }
        }

        /// <summary>
        ///     Parses the stream type from the specified input string.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <returns>The stream type.</returns>
        public static StreamType GetStreamType(string input)
        {
            if (input == null)
            {
                return StreamType.Unknown;
            }

            var indexOfDash = input.IndexOf('/');
            var containerString = input.Substring(indexOfDash, input.Length - indexOfDash - 1);
            return Enum.TryParse(containerString, true, out StreamType type) ? type : StreamType.Unknown;
        }
    }
}
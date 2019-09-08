using System;
using YoutubeLib.Videos;

namespace YoutubeLib.Streams
{
    /// <summary>
    ///     Represents the base class for a Youtube media stream.
    /// </summary>
    [Serializable]
    public abstract class StreamBase
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="StreamBase" /> class with the specified itag, URL and container.
        /// </summary>
        /// <param name="itag">The itag code.</param>
        /// <param name="url">The url that serves the stream.</param>
        /// <param name="streamType">The stream's type/container.</param>
        /// <param name="videoQuality">The stream's video quality.</param>
        protected StreamBase(int itag, string url, StreamType streamType, VideoQuality? videoQuality)
        {
            Itag = itag;
            Url = url;
            StreamType = streamType;
            VideoQuality = videoQuality;
        }

        /// <summary>
        ///     An integer key that describes the stream's properties.
        /// </summary>
        public int Itag { get; }

        /// <summary>
        ///     Gets the stream's type/container.
        /// </summary>
        public StreamType StreamType { get; set; }

        /// <summary>
        ///     Gets the URL that serves the stream.
        /// </summary>
        public string Url { get; }

        /// <summary>
        ///     Gets the stream's video quality.
        /// </summary>
        public VideoQuality? VideoQuality { get; }
    }
}
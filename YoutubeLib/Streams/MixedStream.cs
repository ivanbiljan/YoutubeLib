using YoutubeLib.Videos;

namespace YoutubeLib.Streams
{
    /// <summary>
    ///     Represents a mixed (an AV) YouTube stream.
    /// </summary>
    public sealed class MixedStream : StreamBase
    {
        /// <inheritdoc />
        public MixedStream(int itag, string url, StreamType streamType, VideoQuality? videoQuality) : base(itag, url,
            streamType, videoQuality)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="MixedStream" /> class with the specified stream properties.
        /// </summary>
        /// <param name="itag">The itag code.</param>
        /// <param name="url">The streams URL.</param>
        /// <param name="streamType">The stream's type/media container.</param>
        /// <param name="videoQuality">The stream's video quality.</param>
        /// <param name="audioCodec">The stream's audio codec.</param>
        /// <param name="videoCodec">The stream's video codec.</param>
        public MixedStream(int itag, string url, StreamType streamType, VideoQuality? videoQuality,
            AudioCodec audioCodec, VideoCodec videoCodec) : base(itag, url, streamType, videoQuality)
        {
            AudioCodec = audioCodec;
            VideoCodec = videoCodec;
        }

        /// <summary>
        ///     Gets the audio codec for this stream.
        /// </summary>
        public AudioCodec AudioCodec { get; }

        /// <summary>
        ///     Gets the video codec for this stream.
        /// </summary>
        public VideoCodec VideoCodec { get; }
    }
}
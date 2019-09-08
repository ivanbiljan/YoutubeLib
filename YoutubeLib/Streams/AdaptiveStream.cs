using YoutubeLib.Videos;

namespace YoutubeLib.Streams
{
    /// <summary>
    ///     Represents an adaptive YouTube media stream (video or audio only).
    /// </summary>
    public sealed class AdaptiveStream : StreamBase
    {
        /// <inheritdoc />
        public AdaptiveStream(int itag, string url, StreamType streamType, VideoQuality? videoQuality) : base(itag, url,
            streamType, videoQuality)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="AdaptiveStream" /> class with the specified properties.
        /// </summary>
        /// <param name="itag">The stream's itag code.</param>
        /// <param name="url">The stream's URL.</param>
        /// <param name="streamType">The stream's type/container.</param>
        /// <param name="videoQuality">The stream's video quality.</param>
        /// <param name="adaptiveType">The type of adaptive stream.</param>
        /// <param name="contentLength">The content length of the stream (in bytes).</param>
        /// <param name="fps">The stream's framerate (video only streams).</param>
        /// <param name="bitrate">The video's bitrate (audio only streams).</param>
        /// <param name="resolution">The video resolution (video only streams).</param>
        public AdaptiveStream(int itag, string url, StreamType streamType, VideoQuality? videoQuality,
            AdaptiveType adaptiveType, int contentLength, int fps, int bitrate, Resolution resolution) : base(itag, url,
            streamType, videoQuality)
        {
            AdaptiveType = adaptiveType;
            ContentLength = contentLength;
            Fps = fps;
            Bitrate = bitrate;
            Resolution = resolution;
        }

        /// <summary>
        ///     Gets a value indicating whether the stream is video only or audio only.
        /// </summary>
        public AdaptiveType AdaptiveType { get; }

        /// <summary>
        ///     Gets the audio codec for this stream. This property is <c>null</c> for video only streams.
        /// </summary>
        public AudioCodec? AudioCodec { get; }

        /// <summary>
        ///     Gets the stream bitrate.
        /// </summary>
        public int Bitrate { get; }

        /// <summary>
        ///     Gets the content length in bytes.
        /// </summary>
        public int ContentLength { get; }

        /// <summary>
        ///     Gets the video framerate.
        /// </summary>
        public int Fps { get; }

        /// <summary>
        ///     Gets the video resolution.
        /// </summary>
        public Resolution Resolution { get; }

        /// <summary>
        ///     Gets the video codec for this stream. This property is <c>null</c> for audio only streams.
        /// </summary>
        public VideoCodec? VideoCodec { get; }
    }
}
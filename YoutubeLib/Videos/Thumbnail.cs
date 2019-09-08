using Newtonsoft.Json;

namespace YoutubeLib.Videos
{
    /// <summary>
    ///     Represents a video thumbnail.
    /// </summary>
    public sealed class Thumbnail
    {
        /// <summary>
        ///     Gets the thumbnail's height.
        /// </summary>
        [JsonProperty("height")]
        public int Height { get; private set; }

        /// <summary>
        ///     Gets the thumbnail's URL.
        /// </summary>
        [JsonProperty("url")]
        public string Url { get; private set; }

        /// <summary>
        ///     Gets the thumbnail's width.
        /// </summary>
        [JsonProperty("width")]
        public int Width { get; private set; }
    }
}
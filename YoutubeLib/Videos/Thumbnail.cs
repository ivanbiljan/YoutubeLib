using Newtonsoft.Json;

namespace YoutubeLib.Videos
{
    /// <summary>
    ///     Represents a video thumbnail.
    /// </summary>
    public sealed class Thumbnail
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Thumbnail" /> class with the specified URL, width and height.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public Thumbnail(string url, int width, int height)
        {
            Url = url;
            Width = width;
            Height = height;
        }

        /// <summary>
        ///     Gets the thumbnail's height.
        /// </summary>
        [JsonProperty("height")]
        public int Height { get; }

        /// <summary>
        ///     Gets the thumbnail's URL.
        /// </summary>
        [JsonProperty("url")]
        public string Url { get; }

        /// <summary>
        ///     Gets the thumbnail's width.
        /// </summary>
        [JsonProperty("width")]
        public int Width { get; }
    }
}
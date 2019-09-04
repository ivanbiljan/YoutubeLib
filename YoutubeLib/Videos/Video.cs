using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace YoutubeLib.Videos
{
    /// <summary>
    ///     Represents a YouTube video.
    /// </summary>
    public sealed class Video
    {
        [JsonProperty("keywords")]
        private readonly string[] _keywords;

        /// <summary>
        ///     Gets the video author's channel ID.
        /// </summary>
        [JsonProperty("channelId")]
        public string AuthorId { get; private set; }

        /// <summary>
        ///     Gets the video author's channel name.
        /// </summary>
        [JsonProperty("author")]
        public string AuthorName { get; private set; }

        /// <summary>
        ///     Gets the video's description.
        /// </summary>
        [JsonProperty("shortDescription")]
        public string Description { get; private set; }

        /// <summary>
        ///     Gets the video's ID.
        /// </summary>
        [JsonProperty("videoId")]
        public string Id { get; private set; }

        /// <summary>
        ///     Gets the keywords associated with this video.
        /// </summary>
        public IEnumerable<string> Keywords => _keywords.AsEnumerable();

        /// <summary>
        ///     Gets the length of the video.
        /// </summary>
        [JsonProperty("lengthSeconds")]
        public int LengthSeconds { get; private set; }

        /// <summary>
        ///     Gets the video's thumbnails.
        /// </summary>
        public IEnumerable<Thumbnail> Thumbnails { get; internal set; }

        /// <summary>
        ///     Gets the video's title.
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; private set; }

        /// <summary>
        ///     Gets the video's view count.
        /// </summary>
        [JsonProperty("viewCount")]
        public int ViewCount { get; private set; }

        /// <inheritdoc />
        public override string ToString()
        {
            return
                $"[{Title} ({Id}): AuthorId = {AuthorId} AuthorName = {AuthorName} Description = {Description} Length = {LengthSeconds / 60}:{LengthSeconds % 60} Views: {ViewCount} Keywords = [{string.Join(", ", Keywords)}]]";
        }
    }
}
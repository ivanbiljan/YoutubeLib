using System.Collections.Generic;
using Newtonsoft.Json;

namespace YoutubeLib.Videos
{
    /// <summary>
    ///     Represents a YouTube video.
    /// </summary>
    public sealed class Video
    {
        /// <summary>
        ///     Gets the video author's channel ID.
        /// </summary>
        [JsonProperty("channelId")]
        public string AuthorId { get; }

        /// <summary>
        ///     Gets the video author's channel name.
        /// </summary>
        [JsonProperty("author")]
        public string AuthorName { get; }

        /// <summary>
        ///     Gets the video's description.
        /// </summary>
        [JsonProperty("shortDescription")]
        public string Description { get; }

        /// <summary>
        ///     Gets the video's ID.
        /// </summary>
        [JsonProperty("videoId")]
        public string Id { get; }

        /// <summary>
        ///     Gets the keywords associated with this video.
        /// </summary>
        [JsonProperty("keywords")]
        public IEnumerable<string> Keywords { get; internal set; }

        /// <summary>
        ///     Gets the length of the video.
        /// </summary>
        [JsonProperty("lengthSeconds")]
        public int LengthSeconds { get; }

        /// <summary>
        ///     Gets the video's thumbnails.
        /// </summary>
        public IEnumerable<Thumbnail> Thumbnails { get; internal set; }

        /// <summary>
        ///     Gets the video's title.
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>
        ///     Gets the video's view count.
        /// </summary>
        [JsonProperty("viewCount")]
        public int ViewCount { get; }

        /// <inheritdoc />
        public override string ToString()
        {
            return
                $"[{Title} ({Id}): AuthorId = {AuthorId} AuthorName = {AuthorName} Description = {Description} Length = {LengthSeconds / 60}:{LengthSeconds % 60} Views: {ViewCount} Keywords = [{string.Join(", ", Keywords)}]]";
        }
    }
}
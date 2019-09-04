using System;
using Newtonsoft.Json;

namespace YoutubeLib.Playlists
{
    /// <summary>
    ///     Represents a playlist item/video.
    /// </summary>
    public sealed class PlaylistItem
    {
        [JsonProperty("added")] private string _stringDateAdded;

        [JsonProperty("views")] private string _stringViewCount;

        /// <summary>
        ///     Gets the video's author.
        /// </summary>
        [JsonProperty("author")]
        public string Author { get; private set; }

        /// <summary>
        ///     Gets the video's category ID.
        /// </summary>
        [JsonProperty("categoryId")]
        public CategoryId CategoryId { get; private set; }

        /// <summary>
        ///     Gets the video's upload date.
        /// </summary>
        public DateTime DateAdded => DateTime.Parse(_stringDateAdded);

        /// <summary>
        ///     Gets the video's description.
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; private set; }

        /// <summary>
        ///     Gets the video's number of dislikes.
        /// </summary>
        [JsonProperty("dislikes")]
        public int Dislikes { get; private set; }

        /// <summary>
        ///     Gets the video's duration.
        /// </summary>
        [JsonProperty("duration")]
        public string Duration { get; private set; }

        /// <summary>
        ///     Gets the video's encrypted ID.
        /// </summary>
        [JsonProperty("encrypted_id")]
        public string EncryptedId { get; private set; }

        /// <summary>
        ///     Gets the video's number of likes.
        /// </summary>
        [JsonProperty("likes")]
        public int Likes { get; private set; }

        /// <summary>
        ///     Gets the video's thumbnail URL.
        /// </summary>
        [JsonProperty("thumbnail")]
        public string Thumbnail { get; private set; }

        /// <summary>
        ///     Gets the video's title.
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; private set; }

        /// <summary>
        ///     Gets the video's view count.
        /// </summary>
        public long ViewCount => long.Parse(_stringViewCount.Replace(".", string.Empty));
    }
}
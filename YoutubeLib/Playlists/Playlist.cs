using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace YoutubeLib.Playlists
{
    /// <summary>
    ///     Represents a YouTube playlist.
    /// </summary>
    public sealed class Playlist
    {
        [JsonProperty("video")] private List<PlaylistItem> _videos;

        /// <summary>
        ///     Gets the playlist's description.
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; private set; }

        /// <summary>
        ///     Gets the playlist's title.
        /// </summary>
        [JsonProperty("title")]
        public string Name { get; private set; }

        /// <summary>
        ///     Gets the playlist's videos.
        /// </summary>
        public IEnumerable<PlaylistItem> Videos => _videos.AsEnumerable();

        /// <summary>
        ///     Gets the playlist's view count.
        /// </summary>
        [JsonProperty("views")]
        public int ViewCount { get; private set; }

        internal void AddVideo(PlaylistItem playlistItem)
        {
            _videos.Add(playlistItem);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Text;

namespace YoutubeLib.Channels
{
    /// <summary>
    /// Represents a YouTube channel.
    /// </summary>
    public sealed class Channel
    {
        /// <summary>
        /// Gets the channel's ID.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Gets the channel's canonical base url.
        /// </summary>
        public string CanonicalBaseUrl => $"channel/{Id}";

        /// <summary>
        /// Gets the channel's username.
        /// </summary>
        public string Username { get; }
    }
}

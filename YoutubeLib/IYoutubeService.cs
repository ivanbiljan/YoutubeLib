using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using YoutubeLib.Channels;
using YoutubeLib.Streams;
using YoutubeLib.Videos;

namespace YoutubeLib
{
    /// <summary>
    /// Describes a YouTube service.
    /// </summary>
    public interface IYoutubeService
    {
        /// <summary>
        /// Gets information about a YouTube video using the specified URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>The task for this operation.</returns>
        Task<Video> GetVideoAsync(string url);

        /// <summary>
        /// Gets information about a YouTube channel with the specified channel ID.
        /// </summary>
        /// <param name="channelId">The channel ID.</param>
        /// <returns>The task for this operation.</returns>
        Task<Channel> GetChannelAsync(string channelId);

        /// <summary>
        /// Returns an enumerable collection of media streams for the video specified by the given URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>The task for this operation.</returns>
        Task<IEnumerable<StreamBase>> GetMediaStreamsAsync(string url);

        /// <summary>
        /// Writes the specified YouTube media stream to a given output stream.
        /// </summary>
        /// <param name="mediaStream">The YouTube media stream.</param>
        /// <param name="outputStream">The output stream.</param>
        /// <returns>The task for this operation.</returns>
        Task DownloadMediaStreamAsync(StreamBase mediaStream, Stream outputStream);

        /// <summary>
        /// Writes the specified YouTube media stream to a given file path.
        /// </summary>
        /// <param name="mediaStream">The YouTube media stream.</param>
        /// <param name="filePath">The file path.</param>
        /// <returns>The task for this operation.</returns>
        Task DownloadMediaStreamAsync(StreamBase mediaStream, string filePath);
    }
}

using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using NetStdHttpClient = System.Net.Http.HttpClient;

namespace YoutubeLib.Http
{
    /// <summary>
    ///     Represents the <see cref="System.Net.Http.HttpClient" /> instance.
    /// </summary>
    public static class HttpClient
    {
        private static readonly object SyncLock = new object();
        private static NetStdHttpClient _instance;

        /// <summary>
        ///     Gets the <see cref="System.Net.Http.HttpClient" /> instance.
        /// </summary>
        public static NetStdHttpClient Instance
        {
            get
            {
                lock (SyncLock)
                {
                    if (_instance == null)
                    {
                        // This should resolve 403 responses
                        var handler = new HttpClientHandler
                        {
                            UseCookies = false, UseDefaultCredentials = true,
                            AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
                        };
                        _instance = new NetStdHttpClient(handler, true);
                        _instance.DefaultRequestHeaders.Add("User-Agent", "Random User-Agent header");
                    }

                    return _instance;
                }
            }
        }

        internal static async Task DownloadToFileAsync(string url, string file)
        {
            long contentLength;
            using (var request = new HttpRequestMessage {RequestUri = new System.Uri(url), Method = HttpMethod.Head})
            using (var response = await Instance.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false))
            {
                contentLength = response.Content.Headers.ContentLength.Value;
            }

            using (var stream = await GetStreamInSegmentsAsync(url, contentLength).ConfigureAwait(false))
            using (var fileStream = File.Create(file))
            {
                await stream.CopyToAsync(fileStream);
            }
        }

        internal static async Task<Stream> GetStreamInSegmentsAsync(string url, long contentLength)
        {
            return new HttpHelperStream(url, contentLength);
        }
    }
}
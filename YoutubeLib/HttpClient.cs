using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using NetStdHttpClient = System.Net.Http.HttpClient;

namespace YoutubeLib
{
    /// <summary>
    /// Represents the <see cref="System.Net.Http.HttpClient"/> instance.
    /// </summary>
    public static class HttpClient
    {
        private static readonly object SyncLock = new object();
        private static NetStdHttpClient _instance;

        /// <summary>
        /// Gets the <see cref="System.Net.Http.HttpClient"/> instance.
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
                        var handler = new HttpClientHandler {UseCookies = false, UseDefaultCredentials = true};
                        _instance = new NetStdHttpClient(handler, true);
                        _instance.DefaultRequestHeaders.Add("User-Agent", "Random User-Agent header");
                    }

                    return _instance;
                }
            }
        }

        internal static async Task<Stream> GetStreamInSegmentsAsync(string url, long segmentSize = 10)
        {
            //var request = (HttpWebRequest) WebRequest.Create(url);
            //request.UserAgent =
            //    @"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.106 Safari/537.36";
            //request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8";
            //request.UseDefaultCredentials = true;
            //if (request.ContentLength == 0)
            //{
            //    return null;
            //}

            //var outputStream = new MemoryStream();
            //var response = (HttpWebResponse) await request.GetResponseAsync();
            //response.ContentLength = request.ContentLength;
            //using (var responseStream = response.GetResponseStream())
            //{
            //    if (responseStream == null)
            //    {
            //        return null;
            //    }

            //    segmentSize = segmentSize <= 0 ? (int) responseStream.Length : segmentSize * 1024 * 1024;
            //    var buffer = new byte[segmentSize];

            //    int length;
            //    do
            //    {
            //        length = responseStream.Read(buffer, 0, buffer.Length);
            //        outputStream.Write(buffer, 0, length);
            //    } while (length > 0);
            //}

            //return outputStream;

            var outputStream = new MemoryStream();
            using (var responseStream = await Instance.GetStreamAsync(url))
            {
                segmentSize = segmentSize <= 0 ? responseStream.Length : segmentSize * 1024 * 1024;
                var buffer = new byte[segmentSize];

                int length;
                do
                {
                    length = responseStream.Read(buffer, 0, buffer.Length);
                    outputStream.Write(buffer, 0, length);
                } while (length > 0);
            }

            outputStream.Position = 0;
            return outputStream;
        }

        internal static async Task DownloadToFileAsync(string url, string file, int segmentSize = 10)
        {
            using (var stream = await GetStreamInSegmentsAsync(url, segmentSize))
            using (var fileStream = new FileStream(file, FileMode.Create))
            {
                await stream.CopyToAsync(fileStream);
            }
        }
    }
}

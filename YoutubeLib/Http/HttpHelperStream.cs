using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;

namespace YoutubeLib.Http
{
    internal sealed class HttpHelperStream : Stream
    {
        private const long ChunkSize = 10 * 1024 * 1024;

        private readonly long _contentLength;
        private readonly string _uri;
        private Stream _httpContentStream;
        private long _length;
        private long _position;

        public HttpHelperStream(string uri, long contentLength)
        {
            if (contentLength < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(contentLength));
            }

            _uri = uri ?? throw new ArgumentNullException(nameof(uri));
            _contentLength = contentLength;
        }

        /// <inheritdoc />
        public override bool CanRead => true;

        /// <inheritdoc />
        public override bool CanSeek => true;

        /// <inheritdoc />
        public override bool CanWrite => false;

        /// <inheritdoc />
        public override long Length => _length;

        /// <inheritdoc />
        public override long Position
        {
            get => _position;
            set => throw new NotSupportedException();
        }

        /// <inheritdoc />
        public override void Flush()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override int Read(byte[] buffer, int offset, int count)
        {
            if (_length >= _contentLength)
            {
                return 0;
            }

            _httpContentStream = _httpContentStream ?? GetHttpStreamAsync(_length, _length + ChunkSize - 1);
            var readBytes = _httpContentStream.ReadAsync(buffer, offset, count).ConfigureAwait(false).GetAwaiter()
                .GetResult();
            _length += readBytes;
            if (readBytes != 0)
            {
                return readBytes;
            }

            _httpContentStream?.Dispose();
            _httpContentStream = null;
            return Read(buffer, offset, count);

            Stream GetHttpStreamAsync(long from, long to)
            {
                using (var request = new HttpRequestMessage {RequestUri = new Uri(_uri), Method = HttpMethod.Get})
                {
                    request.Headers.Range = new RangeHeaderValue(from, to);
                    var response = HttpClient.Instance.SendAsync(request, HttpCompletionOption.ResponseHeadersRead)
                        .ConfigureAwait(false)
                        .GetAwaiter().GetResult();
                    return response.Content.ReadAsStreamAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                }
            }
        }

        /// <inheritdoc />
        public override long Seek(long offset, SeekOrigin origin)
        {
            switch (origin)
            {
                case SeekOrigin.Begin:
                    _position = offset;
                    break;
                case SeekOrigin.Current:
                    _position += offset;
                    break;
                case SeekOrigin.End:
                    _position = Length + offset;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(origin), origin, null);
            }

            return _position;
        }

        /// <inheritdoc />
        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }
    }
}
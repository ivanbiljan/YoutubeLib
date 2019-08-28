using System;
using System.Collections.Generic;
using System.Text;

namespace YoutubeLib.Streams
{
    /// <summary>
    /// Specifies the video codec of a YouTube media stream.
    /// </summary>
    public enum VideoCodec
    {
        Unknown,
        Mp4V,
        H263,
        H264,
        Vp8,
        Vp9,
        Av1
    }
}

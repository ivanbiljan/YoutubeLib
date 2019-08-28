using System;
using System.Collections.Generic;
using System.Text;

namespace YoutubeLib.Streams
{
    /// <summary>
    /// Specifies the container for a YouTube media stream.
    /// </summary>
    public enum StreamType
    {
        Unknown,
        Flv, 
        Tgpp,
        Mp4,
        Webm,
        Hls,
        M4a,
    }
}

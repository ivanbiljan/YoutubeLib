using System.Collections.Generic;
using YoutubeLib.Videos;

namespace YoutubeLib.Streams
{
    /// <summary>
    ///     Describes an itag code.
    /// </summary>
    internal sealed class ItagDescriptor
    {
        /// <summary>
        ///     A mapping of itag codes to their corresponding itag descriptors.
        /// </summary>
        public static readonly Dictionary<int, ItagDescriptor> KnownDescriptors = new Dictionary<int, ItagDescriptor>
        {
            // Mixed
            {
                5,
                new ItagDescriptor(StreamType.Flv, Streams.AudioCodec.Mp3,
                    Streams.VideoCodec.H263, VideoQuality.Low144)
            },
            {
                6,
                new ItagDescriptor(StreamType.Flv, Streams.AudioCodec.Mp3,
                    Streams.VideoCodec.H263, VideoQuality.Low240)
            },
            {
                13,
                new ItagDescriptor(StreamType.Tgpp, Streams.AudioCodec.Aac,
                    Streams.VideoCodec.Mp4V, VideoQuality.Low144)
            },
            {
                17,
                new ItagDescriptor(StreamType.Tgpp, Streams.AudioCodec.Aac,
                    Streams.VideoCodec.Mp4V, VideoQuality.Low144)
            },
            {
                18,
                new ItagDescriptor(StreamType.Mp4, Streams.AudioCodec.Aac,
                    Streams.VideoCodec.H264, VideoQuality.Medium360)
            },
            {
                22,
                new ItagDescriptor(StreamType.Mp4, Streams.AudioCodec.Aac,
                    Streams.VideoCodec.H264, VideoQuality.High720)
            },
            {
                34,
                new ItagDescriptor(StreamType.Flv, Streams.AudioCodec.Aac,
                    Streams.VideoCodec.H264, VideoQuality.Medium360)
            },
            {
                35,
                new ItagDescriptor(StreamType.Flv, Streams.AudioCodec.Aac,
                    Streams.VideoCodec.H264, VideoQuality.Medium480)
            },
            {
                36,
                new ItagDescriptor(StreamType.Tgpp, Streams.AudioCodec.Aac,
                    Streams.VideoCodec.Mp4V, VideoQuality.Low240)
            },
            {
                37,
                new ItagDescriptor(StreamType.Mp4, Streams.AudioCodec.Aac,
                    Streams.VideoCodec.H264, VideoQuality.High1080)
            },
            {
                38,
                new ItagDescriptor(StreamType.Mp4, Streams.AudioCodec.Aac,
                    Streams.VideoCodec.H264, VideoQuality.High3072)
            },
            {
                43,
                new ItagDescriptor(StreamType.Webm, Streams.AudioCodec.Vorbis,
                    Streams.VideoCodec.Vp8, VideoQuality.Medium360)
            },
            {
                44,
                new ItagDescriptor(StreamType.Webm, Streams.AudioCodec.Vorbis,
                    Streams.VideoCodec.Vp8, VideoQuality.Medium480)
            },
            {
                45,
                new ItagDescriptor(StreamType.Webm, Streams.AudioCodec.Vorbis,
                    Streams.VideoCodec.Vp8, VideoQuality.High720)
            },
            {
                46,
                new ItagDescriptor(StreamType.Webm, Streams.AudioCodec.Vorbis,
                    Streams.VideoCodec.Vp8, VideoQuality.High1080)
            },
            {
                59,
                new ItagDescriptor(StreamType.Mp4, Streams.AudioCodec.Aac,
                    Streams.VideoCodec.H264, VideoQuality.Medium480)
            },
            {
                78,
                new ItagDescriptor(StreamType.Mp4, Streams.AudioCodec.Aac,
                    Streams.VideoCodec.H264, VideoQuality.Medium480)
            },
            {
                82,
                new ItagDescriptor(StreamType.Mp4, Streams.AudioCodec.Aac,
                    Streams.VideoCodec.H264, VideoQuality.Medium360)
            },
            {
                83,
                new ItagDescriptor(StreamType.Mp4, Streams.AudioCodec.Aac,
                    Streams.VideoCodec.H264, VideoQuality.Medium480)
            },
            {
                84,
                new ItagDescriptor(StreamType.Mp4, Streams.AudioCodec.Aac,
                    Streams.VideoCodec.H264, VideoQuality.High720)
            },
            {
                85,
                new ItagDescriptor(StreamType.Mp4, Streams.AudioCodec.Aac,
                    Streams.VideoCodec.H264, VideoQuality.High1080)
            },
            {
                91,
                new ItagDescriptor(StreamType.Mp4, Streams.AudioCodec.Aac,
                    Streams.VideoCodec.H264, VideoQuality.Low144)
            },
            {
                92,
                new ItagDescriptor(StreamType.Mp4, Streams.AudioCodec.Aac,
                    Streams.VideoCodec.H264, VideoQuality.Low240)
            },
            {
                93,
                new ItagDescriptor(StreamType.Mp4, Streams.AudioCodec.Aac,
                    Streams.VideoCodec.H264, VideoQuality.Medium360)
            },
            {
                94,
                new ItagDescriptor(StreamType.Mp4, Streams.AudioCodec.Aac,
                    Streams.VideoCodec.H264, VideoQuality.Medium480)
            },
            {
                95,
                new ItagDescriptor(StreamType.Mp4, Streams.AudioCodec.Aac,
                    Streams.VideoCodec.H264, VideoQuality.High720)
            },
            {
                96,
                new ItagDescriptor(StreamType.Mp4, Streams.AudioCodec.Aac,
                    Streams.VideoCodec.H264, VideoQuality.High1080)
            },
            {
                100,
                new ItagDescriptor(StreamType.Webm, Streams.AudioCodec.Vorbis,
                    Streams.VideoCodec.Vp8, VideoQuality.Medium360)
            },
            {
                101,
                new ItagDescriptor(StreamType.Webm, Streams.AudioCodec.Vorbis,
                    Streams.VideoCodec.Vp8, VideoQuality.Medium480)
            },
            {
                102,
                new ItagDescriptor(StreamType.Webm, Streams.AudioCodec.Vorbis,
                    Streams.VideoCodec.Vp8, VideoQuality.High720)
            },
            {
                132,
                new ItagDescriptor(StreamType.Mp4, Streams.AudioCodec.Aac,
                    Streams.VideoCodec.H264, VideoQuality.Low240)
            },
            {
                151,
                new ItagDescriptor(StreamType.Mp4, Streams.AudioCodec.Aac,
                    Streams.VideoCodec.H264, VideoQuality.Low144)
            },

            // Video-only (mp4)
            {133, new ItagDescriptor(StreamType.Mp4, null, Streams.VideoCodec.H264, VideoQuality.Low240)},
            {134, new ItagDescriptor(StreamType.Mp4, null, Streams.VideoCodec.H264, VideoQuality.Medium360)},
            {135, new ItagDescriptor(StreamType.Mp4, null, Streams.VideoCodec.H264, VideoQuality.Medium480)},
            {136, new ItagDescriptor(StreamType.Mp4, null, Streams.VideoCodec.H264, VideoQuality.High720)},
            {137, new ItagDescriptor(StreamType.Mp4, null, Streams.VideoCodec.H264, VideoQuality.High1080)},
            {138, new ItagDescriptor(StreamType.Mp4, null, Streams.VideoCodec.H264, VideoQuality.High4320)},
            {160, new ItagDescriptor(StreamType.Mp4, null, Streams.VideoCodec.H264, VideoQuality.Low144)},
            {212, new ItagDescriptor(StreamType.Mp4, null, Streams.VideoCodec.H264, VideoQuality.Medium480)},
            {213, new ItagDescriptor(StreamType.Mp4, null, Streams.VideoCodec.H264, VideoQuality.Medium480)},
            {214, new ItagDescriptor(StreamType.Mp4, null, Streams.VideoCodec.H264, VideoQuality.High720)},
            {215, new ItagDescriptor(StreamType.Mp4, null, Streams.VideoCodec.H264, VideoQuality.High720)},
            {216, new ItagDescriptor(StreamType.Mp4, null, Streams.VideoCodec.H264, VideoQuality.High1080)},
            {217, new ItagDescriptor(StreamType.Mp4, null, Streams.VideoCodec.H264, VideoQuality.High1080)},
            {264, new ItagDescriptor(StreamType.Mp4, null, Streams.VideoCodec.H264, VideoQuality.High1440)},
            {266, new ItagDescriptor(StreamType.Mp4, null, Streams.VideoCodec.H264, VideoQuality.High2160)},
            {298, new ItagDescriptor(StreamType.Mp4, null, Streams.VideoCodec.H264, VideoQuality.High720)},
            {299, new ItagDescriptor(StreamType.Mp4, null, Streams.VideoCodec.H264, VideoQuality.High1080)},

            // Video-only (Webm)
            {167, new ItagDescriptor(StreamType.Webm, null, Streams.VideoCodec.Vp8, VideoQuality.Medium360)},
            {168, new ItagDescriptor(StreamType.Webm, null, Streams.VideoCodec.Vp8, VideoQuality.Medium480)},
            {169, new ItagDescriptor(StreamType.Webm, null, Streams.VideoCodec.Vp8, VideoQuality.High720)},
            {170, new ItagDescriptor(StreamType.Webm, null, Streams.VideoCodec.Vp8, VideoQuality.High1080)},
            {218, new ItagDescriptor(StreamType.Webm, null, Streams.VideoCodec.Vp8, VideoQuality.Medium480)},
            {219, new ItagDescriptor(StreamType.Webm, null, Streams.VideoCodec.Vp8, VideoQuality.Medium480)},
            {242, new ItagDescriptor(StreamType.Webm, null, Streams.VideoCodec.Vp9, VideoQuality.Low240)},
            {243, new ItagDescriptor(StreamType.Webm, null, Streams.VideoCodec.Vp9, VideoQuality.Medium360)},
            {244, new ItagDescriptor(StreamType.Webm, null, Streams.VideoCodec.Vp9, VideoQuality.Medium480)},
            {245, new ItagDescriptor(StreamType.Webm, null, Streams.VideoCodec.Vp9, VideoQuality.Medium480)},
            {246, new ItagDescriptor(StreamType.Webm, null, Streams.VideoCodec.Vp9, VideoQuality.Medium480)},
            {247, new ItagDescriptor(StreamType.Webm, null, Streams.VideoCodec.Vp9, VideoQuality.High720)},
            {248, new ItagDescriptor(StreamType.Webm, null, Streams.VideoCodec.Vp9, VideoQuality.High1080)},
            {271, new ItagDescriptor(StreamType.Webm, null, Streams.VideoCodec.Vp9, VideoQuality.High1440)},
            {272, new ItagDescriptor(StreamType.Webm, null, Streams.VideoCodec.Vp9, VideoQuality.High2160)},
            {278, new ItagDescriptor(StreamType.Webm, null, Streams.VideoCodec.Vp9, VideoQuality.Low144)},
            {302, new ItagDescriptor(StreamType.Webm, null, Streams.VideoCodec.Vp9, VideoQuality.High720)},
            {303, new ItagDescriptor(StreamType.Webm, null, Streams.VideoCodec.Vp9, VideoQuality.High1080)},
            {308, new ItagDescriptor(StreamType.Webm, null, Streams.VideoCodec.Vp9, VideoQuality.High1440)},
            {313, new ItagDescriptor(StreamType.Webm, null, Streams.VideoCodec.Vp9, VideoQuality.High2160)},
            {315, new ItagDescriptor(StreamType.Webm, null, Streams.VideoCodec.Vp9, VideoQuality.High2160)},
            {330, new ItagDescriptor(StreamType.Webm, null, Streams.VideoCodec.Vp9, VideoQuality.Low144)},
            {331, new ItagDescriptor(StreamType.Webm, null, Streams.VideoCodec.Vp9, VideoQuality.Low240)},
            {332, new ItagDescriptor(StreamType.Webm, null, Streams.VideoCodec.Vp9, VideoQuality.Medium360)},
            {333, new ItagDescriptor(StreamType.Webm, null, Streams.VideoCodec.Vp9, VideoQuality.Medium480)},
            {334, new ItagDescriptor(StreamType.Webm, null, Streams.VideoCodec.Vp9, VideoQuality.High720)},
            {335, new ItagDescriptor(StreamType.Webm, null, Streams.VideoCodec.Vp9, VideoQuality.High1080)},
            {336, new ItagDescriptor(StreamType.Webm, null, Streams.VideoCodec.Vp9, VideoQuality.High1440)},
            {337, new ItagDescriptor(StreamType.Webm, null, Streams.VideoCodec.Vp9, VideoQuality.High2160)},

            // Audio-only (mp4)
            {139, new ItagDescriptor(StreamType.M4a, Streams.AudioCodec.Aac, null, null)},
            {140, new ItagDescriptor(StreamType.M4a, Streams.AudioCodec.Aac, null, null)},
            {141, new ItagDescriptor(StreamType.M4a, Streams.AudioCodec.Aac, null, null)},
            {256, new ItagDescriptor(StreamType.M4a, Streams.AudioCodec.Aac, null, null)},
            {258, new ItagDescriptor(StreamType.M4a, Streams.AudioCodec.Aac, null, null)},
            {325, new ItagDescriptor(StreamType.M4a, Streams.AudioCodec.Aac, null, null)},
            {328, new ItagDescriptor(StreamType.M4a, Streams.AudioCodec.Aac, null, null)},

            // Audio-only (Webm)
            {171, new ItagDescriptor(StreamType.Webm, Streams.AudioCodec.Vorbis, null, null)},
            {172, new ItagDescriptor(StreamType.Webm, Streams.AudioCodec.Vorbis, null, null)},
            {249, new ItagDescriptor(StreamType.Webm, Streams.AudioCodec.Opus, null, null)},
            {250, new ItagDescriptor(StreamType.Webm, Streams.AudioCodec.Opus, null, null)},
            {251, new ItagDescriptor(StreamType.Webm, Streams.AudioCodec.Opus, null, null)}
        };

        /// <summary>
        ///     Gets the default itag descriptor used for unknown itag codes.
        /// </summary>
        public static ItagDescriptor DefaultDescriptor = new ItagDescriptor(StreamType.Unknown,
            Streams.AudioCodec.Unknown, Streams.VideoCodec.Unknown, null);

        /// <summary>
        ///     Initializes a new instance of the <see cref="ItagDescriptor" /> class with the specified properties.
        /// </summary>
        /// <param name="streamType">The type of stream.</param>
        /// <param name="audioCodec">The audio codec.</param>
        /// <param name="videoCodec">The video codec.</param>
        /// <param name="quality">The video quality.</param>
        public ItagDescriptor(StreamType streamType, AudioCodec? audioCodec, VideoCodec? videoCodec,
            VideoQuality? quality)
        {
            StreamType = streamType;
            AudioCodec = audioCodec;
            VideoCodec = videoCodec;
            Quality = quality;
        }

        /// <summary>
        ///     Gets the audio codec.
        /// </summary>
        public AudioCodec? AudioCodec { get; }

        /// <summary>
        ///     Gets the quality.
        /// </summary>
        public VideoQuality? Quality { get; }

        /// <summary>
        ///     Gets the type of stream.
        /// </summary>
        public StreamType StreamType { get; }

        /// <summary>
        ///     Gets the video codec.
        /// </summary>
        public VideoCodec? VideoCodec { get; }
    }
}
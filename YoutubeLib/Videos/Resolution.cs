using System;
using System.Collections.Generic;
using System.Text;

namespace YoutubeLib.Videos
{
    /// <summary>
    /// Represents a video resolution.
    /// </summary>
    public sealed class Resolution
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Resolution"/> class with the specified width and height.
        /// </summary>
        /// <param name="width">The width, which must be greater than zero.</param>
        /// <param name="height">The height, which must be greater than zero.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="width"/> or <paramref name="height"/> is less than or equal to zero.</exception>
        public Resolution(int width, int height)
        {
            if (width <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(width));
            }

            if (height <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(height));
            }

            Width = width;
            Height = height;
        }

        /// <summary>
        /// Gets the width.
        /// </summary>
        public int Width { get; }

        /// <summary>
        /// Gets the height.
        /// </summary>
        public int Height { get; }
    }
}

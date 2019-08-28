using System;
using System.Collections.Generic;
using System.Text;

namespace YoutubeLib.Extensions
{
    /// <summary>
    /// Provides extension methods for the <see cref="string"/> type.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Swaps the characters at the specified positions in the specified input string.
        /// </summary>
        /// <param name="source">The input string, which must not be <c>null</c>.</param>
        /// <param name="firstCharIndex">The index of the first character, which must not be negative.</param>
        /// <param name="secondCharIndex">The index of the second character, which must not be negative.</param>
        /// <returns>The new string.</returns>
        public static string SwapCharacters(this string source, int firstCharIndex, int secondCharIndex)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (firstCharIndex < 0 || firstCharIndex >= source.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(firstCharIndex));
            }

            if (secondCharIndex < 0 || secondCharIndex >= source.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(secondCharIndex));
            }

            var chars = source.ToCharArray();
            var temp = chars[firstCharIndex];
            chars[firstCharIndex] = chars[secondCharIndex];
            chars[secondCharIndex] = temp;
            return new string(chars);
        }

        /// <summary>
        /// Replaces the format items in the specified string with the string representations of the objects in the specified array.
        /// </summary>
        /// <param name="source">The source string, which must not be <c>null</c>.</param>
        /// <param name="args">The arguments.</param>
        /// <returns>The formatted string.</returns>
        public static string Format(this string source, params object[] args)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return string.Format(source, args);
        }
    }
}

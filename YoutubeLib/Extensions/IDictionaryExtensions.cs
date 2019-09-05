using System;
using System.Collections.Generic;

namespace YoutubeLib.Extensions
{
    /// <summary>
    ///     Provides extension methods for the <see cref="IDictionary{TKey,TValue}" /> type.
    /// </summary>
    public static class IDictionaryExtensions
    {
        /// <summary>
        ///     Returns the value of the specified key in the dictionary, or a default value if the key is not present in the
        ///     dictionary.
        /// </summary>
        /// <typeparam name="TKey">The type of key.</typeparam>
        /// <typeparam name="TValue">The type of value.</typeparam>
        /// <param name="dictionary">The dictionary, which must not be <c>null</c>.</param>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">An optional default value.</param>
        /// <returns>
        ///     The value of the specified key in the dictionary, or a default value if the key is not present in the
        ///     dictionary.
        /// </returns>
        public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key,
            TValue defaultValue = default)
        {
            if (dictionary == null)
            {
                throw new ArgumentNullException(nameof(dictionary));
            }

            return dictionary.TryGetValue(key, out var value) ? value : defaultValue;
        }
    }
}
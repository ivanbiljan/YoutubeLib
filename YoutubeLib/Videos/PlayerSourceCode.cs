using System.Collections.Generic;
using System.Linq;
using YoutubeLib.Videos.Deciphering;

namespace YoutubeLib.Videos
{
    /// <summary>
    ///     Represents a YouTube player's source code.
    /// </summary>
    internal sealed class PlayerSourceCode
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="PlayerSourceCode" /> class with the specified session token and
        ///     decipher functions.
        /// </summary>
        /// <param name="sts">The session token.</param>
        /// <param name="decipherFunctions">The decipher functions.</param>
        internal PlayerSourceCode(string sts, IEnumerable<DecipherFunctionBase> decipherFunctions)
        {
            Sts = sts;
            DecipherFunctions = decipherFunctions;
        }

        /// <summary>
        ///     Gets the functions used to decipher the player's signature.
        /// </summary>
        public IEnumerable<DecipherFunctionBase> DecipherFunctions { get; }

        /// <summary>
        ///     Gets a "session token" for this player.
        /// </summary>
        public string Sts { get; }

        /// <summary>
        ///     Applies decipher operations and returns the modified signature.
        /// </summary>
        /// <param name="cipher">The ciphered signature.</param>
        /// <returns>The deciphered signature.</returns>
        public string ApplyDecipherOperations(string cipher)
        {
            return DecipherFunctions.Aggregate(cipher, (curr, func) => func.Execute(curr));
        }
    }
}
namespace YoutubeLib.Videos.Deciphering
{
    /// <summary>
    ///     Provides the base class for a decipher function.
    /// </summary>
    internal abstract class DecipherFunctionBase
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="DecipherFunctionBase" /> class with the specified optional index.
        /// </summary>
        /// <param name="index">The index.</param>
        protected DecipherFunctionBase(int? index = null)
        {
            Index = index;
        }

        /// <summary>
        ///     Gets the index. This value is used with the swap and slice operations.
        /// </summary>
        protected int? Index { get; }

        ///// <summary>
        /////     Gets the signature that needs to be deciphered.
        ///// </summary>
        //protected string Signature { get; }

        /// <summary>
        ///     Executes the operation and returns the modified signature.
        /// </summary>
        /// <param name="signature">The signature that needs to be deciphered.</param>
        public abstract string Execute(string signature);
    }
}
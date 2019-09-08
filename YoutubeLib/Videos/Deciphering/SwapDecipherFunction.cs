using YoutubeLib.Extensions;

namespace YoutubeLib.Videos.Deciphering
{
    internal sealed class SwapDecipherFunction : DecipherFunctionBase
    {
        /// <inheritdoc />
        public SwapDecipherFunction(int? index = null) : base(index)
        {
        }

        /// <inheritdoc />
        public override string Execute(string signature)
        {
            return signature.SwapCharacters(0, Index.Value);
        }
    }
}
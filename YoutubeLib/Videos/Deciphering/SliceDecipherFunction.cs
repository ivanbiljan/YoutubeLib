namespace YoutubeLib.Videos.Deciphering
{
    internal sealed class SliceDecipherFunction : DecipherFunctionBase
    {
        /// <inheritdoc />
        public SliceDecipherFunction(int? index = null) : base(index)
        {
        }

        /// <inheritdoc />
        public override string Execute(string signature)
        {
            return signature.Substring(Index.Value);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YoutubeLib.Videos.Deciphering
{
    internal sealed class ReverseDecipherFunction : DecipherFunctionBase
    {
        /// <inheritdoc />
        public ReverseDecipherFunction(int? index = null) : base(index)
        {
        }

        /// <inheritdoc />
        public override string Execute(string signature)
        {
            return new string(signature.Reverse().ToArray());
        }
    }
}

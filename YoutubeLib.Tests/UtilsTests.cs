using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace YoutubeLib.Tests
{
    [TestFixture]
    public sealed class UtilsTests
    {
        //private static readonly string[] InvalidVideoIds =
        //{
        //    "", "Hello, World!", "youtub.e/123", "youtube.com/watch=abc", "youtube.com/v=456",
        //    "youtube.com/watch&v=def"
        //};

        //private static readonly string[] ValidVideoIds =
        //{
        //    "www.youtu.be/ifXalt3MJtM",
        //    "https://youtu.be/ifXalt3MJtM",
        //    "youtube.com/watch?v=ifXalt3MJtM",
        //    "https://www.youtube.com/v/ifXalt3MJtM"
        //};

        //[TestCaseSource(nameof(ValidVideoIds))]
        //public void ExtractVideoId_IsCorrect(string videoId)
        //{
        //    Assert.AreEqual("ifXalt3MJtM", Utils.ExtractVideoId(videoId));
        //}

        //[TestCaseSource(nameof(InvalidVideoIds))]
        //public void ExtractVideoId_InvalidId_ReturnsStringEmpty(string videoId)
        //{
        //    Assert.AreEqual(string.Empty, Utils.ExtractVideoId(videoId));
        //}
    }
}

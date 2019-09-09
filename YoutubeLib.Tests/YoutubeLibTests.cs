using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using HtmlAgilityPack;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using YoutubeLib;
using YoutubeLib.Streams;

namespace YoutubeLib.Tests
{
    [TestFixture]
    public class YoutubeLibTests
    {
        [Test]
        public async Task TestMethod()
        {
            var youtubeService = new YoutubeService();
            var searchResults = await youtubeService.GetVideosAsync("Corey Taylor");
            Debug.WriteLine(searchResults.ElementAt(0).Title);
        }
    }
}

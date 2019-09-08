using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using YoutubeLib;
using YoutubeLib.Streams;

namespace YoutubeDownloader
{
    internal class Program
    {
        private static readonly Regex InvalidCharactersRegex =
            new Regex($"[{new string(Path.GetInvalidFileNameChars())}]");

        private static readonly string OutputFolderPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
            "YoutubeDownloader");

        private static async Task Main(string[] args)
        {
            Directory.CreateDirectory(OutputFolderPath);

            var youtubeService = new YoutubeService();
            Console.Write("Playlist Link:");
            var playlist = await youtubeService.GetPlaylistAsync(Console.ReadLine());
            foreach (var video in playlist.Videos)
            {
                var stream = (await youtubeService.GetMediaStreamsAsync($"youtu.be/{video.EncryptedId}"))
                    .OfType<MixedStream>().FirstOrDefault();
                if (stream == null)
                {
                    var oldForegroundColor = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"No mixed streams found for video \"{video.Title}\"");
                    Console.ForegroundColor = oldForegroundColor;
                    continue;
                }

                Console.WriteLine($"Downloading {video.Title}");
                await youtubeService.DownloadMediaStreamAsync(stream,
                    Path.Combine(OutputFolderPath, $"{InvalidCharactersRegex.Replace(video.Title, string.Empty)}.mp3"));
                Console.WriteLine("\t--> Done");
            }

            Console.ReadKey();
        }
    }
}
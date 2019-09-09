# YoutubeLib
A standalone .NET Standard library which provides an easy and efficient way to query YouTube for video and media stream information, and download videos. It parses data by deserialiazing JSON objects returned by AJAX requests and does not interfere with Google's YouTube API.

## Features
* Targets .NET Standard 2.0
* Querying YouTube search results, playlists and videos
* Extracting YouTube media stream metadata
* Downloading YouTube media streams
* Does not require an API key to issue requests

## Examples
To get started, you need to construct a new `YoutubeService` instance:
```csharp
static async Task Main(string[] args)
{
    var youtubeService = new YoutubeService();
}
```

### Loading search results for a specified query
At the moment, YoutubeLib returns search results for the front page only (cca. 20 videos). The `GetVideosAsync(string)` method returns metadata for these videos.
```csharp
static async Task Main(string[] args)
{
    var youtubeService = new YoutubeService();
    var searchResults = await youtubeService.GetVideosAsync("Search query");
    foreach (var video in searchResults)
    {
      Console.WriteLine($"{video.Title} (by {video.Author}): {video.Likes} likes and {video.Dislikes} dislikes");
    }
}
```

### Retrieving playlist content
The `GetPlaylistAsync(string)` returns the playlist's content and all its metadata (title, description, author):
```csharp
static async Task Main(string[] args)
{
    var youtubeService = new YoutubeService();
    
    // Get the first 20 results for a speciifed search query
    var searchResults = await youtubeService.GetVideosAsync("Search query");
    foreach (var video in searchResults)
    {
      Console.WriteLine($"{video.Title} (by {video.Author}): {video.Likes} likes and {video.Dislikes} dislikes");
    }
    
    // Get playlist information
    var playlist = await youtubeService.GetPlaylistAsync("https://www.youtube.com/?list=playlistID");
    Console.WriteLine($"Playlist: {playlist.Title} (by {playlist.Author}. Video count: {playlist.Videos.Count()}");
}
```

### Obtaining video information
You can obtain metadata about an individual video by invoking the `GetVideoAsync(string)` method and passing a valid YouTube URL
```csharp
static async Task Main(string[] args)
{
    var youtubeService = new YoutubeService();
    
    // Get the first 20 results for a speciifed search query
    var searchResults = await youtubeService.GetVideosAsync("Search query");
    foreach (var video in searchResults)
    {
      Console.WriteLine($"{video.Title} (by {video.Author}): {video.Likes} likes and {video.Dislikes} dislikes");
    }
    
    // Get playlist information
    var playlist = await youtubeService.GetPlaylistAsync("https://www.youtube.com/?list=playlistID");
    Console.WriteLine($"Playlist: {playlist.Title} (by {playlist.Author}. Video count: {playlist.Videos.Count()}");
    
    // Get video information
    var video = await youtubeService.GetVideoAsync("youtu.be/videoID");
}
```
>Note: YoutubeLib supports the following authorities: youtube.com/watch?v=ID, youtube.com/v/ID, and youtu.be/ID

### Downloading media streams
In order to download a media stream you must first retrieve all media stream information by invoking the `GetMediaStreamsAsync(string)` method. Once complete, you may specify which type of stream you'd like to download (mixed or adaptive) and pass it as an argument to the `DownloadMediaStreamAsync()` method:
```csharp
static async Task Main(string[] args)
{
    var youtubeService = new YoutubeService();
    
    // Get the first 20 results for a speciifed search query
    var searchResults = await youtubeService.GetVideosAsync("Search query");
    foreach (var video in searchResults)
    {
      Console.WriteLine($"{video.Title} (by {video.Author}): {video.Likes} likes and {video.Dislikes} dislikes");
    }
    
    // Get playlist information
    var playlist = await youtubeService.GetPlaylistAsync("https://www.youtube.com/?list=playlistID");
    Console.WriteLine($"Playlist: {playlist.Title} (by {playlist.Author}. Video count: {playlist.Videos.Count()}");
    
    // Get video information
    var video = await youtubeService.GetVideoAsync("youtu.be/videoID");
    
    // Get all media streams
    var mediaStreams = await youtubeService.GetMediaStreamsAsync(video.Id);
    
    // Get a mixed stream and download it
    var streamToDownload = mediaStreams.OfType<MixedStream>().FirstOrDefault();
    if (streamToDownload != null) // There are certain cases where videos do not provide any mixed streams
    {
        await youtubeService.DownloadMediaStreamAsync(streamToDownload, "Test.mp3");
    }
}
```
## Credits
 * Tyrrrz's blog post: https://tyrrrz.me/Blog/Reverse-engineering-YouTube
 * Google's Developer Documentation
 
## TODO List
 * Improve overall performance

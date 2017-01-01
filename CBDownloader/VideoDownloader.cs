using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CBDownloader
{
    public class VideoDownloader
    {
        MyWebClient MyWebClient { get; }
        MyFileStream MyFileStream { get; }

        List<string> TriedChunks { get; }
        List<string> DownloadedChunks { get; }
        List<string> FailedChunks { get; }
        int RepeatedExceptions { get; set; }
        string DownloadPath { get; }
        Uri PlaylistUri { get; }
        Uri BaseUri { get; }
        Uri ChunklistUri { get; set; }

        public VideoDownloader()
        {
            MyWebClient = new MyWebClient();
            MyFileStream = new MyFileStream();

            TriedChunks = new List<string>();
            DownloadedChunks = new List<string>();
            FailedChunks = new List<string>();
            RepeatedExceptions = 0;
            DownloadPath = MyFileStream.GetDownloadPath();
            PlaylistUri = MyFileStream.GetPlaylistUri();
            BaseUri = new Uri(PlaylistUri, ".");
        }

        public async Task DownloadPlaylist()
        {
            await GenerateChunklistUri(PlaylistUri);

            MyFileStream.CreateDirectory(DownloadPath);
            MyFileStream.SavePlaylistFile(DownloadPath, PlaylistUri);

            while (RepeatedExceptions < Constraints.RepeatedExceptionCountToBreak)
            {
                await DownloadChunks();
            }
        }

        async Task GenerateChunklistUri(Uri playlistUri)
        {
            var playlistContents = await MyWebClient.GetFileContent(playlistUri);
            var chunkFile = playlistContents.FirstOrDefault(c => c.EndsWith(Constraints.PlaylistFileExtension, Constraints.StringComparisonOption));
            if (chunkFile != null)
            {
                await GenerateChunklistUri(new Uri(BaseUri, chunkFile));
            }
            else
            {
                ChunklistUri = playlistUri;
            }
        }

        async Task DownloadChunks()
        {
            try
            {
                var chunks = await GetChunksToDownload();
                foreach (var chunk in chunks)
                {
                    DownloadChunk(chunk);
                }
            }
            catch (Exception ex)
            {
                RepeatedExceptions++;
                Console.WriteLine(ex.Message);
            }
        }

        async Task<List<string>> GetChunksToDownload()
        {
            var allchunks = await MyWebClient.GetFileContent(ChunklistUri);
            Console.WriteLine(allchunks.Count + " chunks");
            return allchunks
                .Where(c => !DownloadedChunks.Contains(c) &&
                       !TriedChunks.Contains(c) &&
                       FailedChunks.Count(f => f == c) <= Constraints.RepeatedExceptionCountToBreak)
                .ToList();
        }

        void DownloadChunk(string chunk)
        {
            try
            {
                TriedChunks.Add(chunk);
                MyWebClient.DownloadData(new Uri(BaseUri, chunk)).ContinueWith(data =>
                {
                    MyFileStream.SaveData(data.Result, DownloadPath, chunk);
                    DownloadedChunks.Add(chunk);
                    RepeatedExceptions = 0;
                });
            }
            catch (Exception ex)
            {
                TriedChunks.Remove(chunk);
                FailedChunks.Add(chunk);
                Console.WriteLine(ex.Message);
            }
        }
    }
}
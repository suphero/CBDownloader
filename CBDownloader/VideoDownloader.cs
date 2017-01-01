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
            MyFileStream.CreateDirectory(DownloadPath);
            MyFileStream.SavePlaylistFile(DownloadPath, PlaylistUri);

            while (RepeatedExceptions < Constraints.RepeatedExceptionCountToBreak)
            {
                await DownloadChunks();
            }
        }

		async Task<List<string>> GetChunks(Uri playlistUri) 
		{
			var playlistContents = await MyWebClient.GetFileContent(playlistUri);
			var chunkFile = playlistContents.Where(c => c.EndsWith(Constraints.PlaylistFileExtension, Constraints.StringComparisonOption)).ToList();
			if (chunkFile.Any())
			{
				var chunkUri = new Uri(BaseUri, chunkFile.First());
				return await GetChunks(chunkUri);
			}
			else 
			{
				return playlistContents;
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
			var allchunks = await GetChunks(PlaylistUri);
            Console.WriteLine(allchunks.Count + " chunks");
			var chunksToReturn = new List<string>();
			foreach (var chunk in allchunks)
			{
				var reChunk = Common.GetRegExChunk(chunk);
				if (!DownloadedChunks.Contains(reChunk) &&
				    !TriedChunks.Contains(reChunk) &&
				    FailedChunks.Count(f => f == reChunk) <= Constraints.RepeatedExceptionCountToBreak) 
				{
					chunksToReturn.Add(chunk);
				}
			}

			return chunksToReturn;
        }

        void DownloadChunk(string chunk)
        {
			var reChunk = Common.GetRegExChunk(chunk);
            try
            {
                TriedChunks.Add(reChunk);
                MyWebClient.DownloadData(new Uri(BaseUri, chunk)).ContinueWith(data =>
                {
                    MyFileStream.SaveData(data.Result, DownloadPath, chunk);
                    DownloadedChunks.Add(reChunk);
                    RepeatedExceptions = 0;
                });
            }
            catch (Exception ex)
            {
                TriedChunks.Remove(reChunk);
                FailedChunks.Add(reChunk);
                Console.WriteLine(ex.Message);
            }
        }
    }
}
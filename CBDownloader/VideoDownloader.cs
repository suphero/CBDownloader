using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace CBDownloader
{
    public abstract class VideoDownloader
    {
        private readonly WebClient _webClient;

        protected VideoDownloader(WebClient webClient)
        {
            _webClient = webClient;
        }

        public void DownloadPlaylist(Uri playlistUri)
        {
            var baseUri = new Uri(playlistUri, ".");
            var chunkFile = ChunklistFile(playlistUri);
            var chunklistUrl = new Uri(baseUri, chunkFile);
            var downloadedChunks = new List<string>();

            var path = DownloadPath(playlistUri);
            Directory.CreateDirectory(path);

            var error = false;
            while (!error)
            {
                try
                {
                    var allchunks = AllChunks(chunklistUrl);
                    Console.WriteLine(allchunks.Count + " chunks");
                    var chunks = allchunks.Except(downloadedChunks);

                    foreach (var chunk in chunks)
                    {
                        try
                        {
                            var data = _webClient.DownloadData(new Uri(baseUri, chunk));
                            SaveData(data, path, chunk);
                            downloadedChunks.Add(chunk);
                            Console.WriteLine(chunk + " downloaded");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
                catch (Exception ex)
                {
                    error = true;
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private List<string> AllChunks(Uri chunklistUri)
        {
            var chunkList = _webClient.DownloadString(chunklistUri).Trim();
            return GetUncommentedPlaylistLines(chunkList);
        }

        private string ChunklistFile(Uri playlistUri)
        {
            var playlist = _webClient.DownloadString(playlistUri).Trim();
            return GetUncommentedPlaylistLines(playlist).First();
        }

        private List<string> GetUncommentedPlaylistLines(string playlist)
        {
            return playlist.Split('\n').ToList().Select(s => s.Trim()).Where(s => !s.StartsWith("#")).ToList();
        }

        protected abstract string DownloadPath(Uri playlistUri);

        private void SaveData(byte[] buffer, string path, string fileName)
        {
            using (var fileStream = File.Create(Path.Combine(path, fileName)))
            {
                fileStream.Write(buffer, 0, buffer.Length);
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace CBDownloader
{
    public class MyWebClient
    {
        public async Task<List<string>> GetFileContent(Uri playlistUri)
        {
            using (WebClient webClient = new WebClient())
            {
                string playlist = (await webClient.DownloadStringTaskAsync(playlistUri)).Trim();
                return Common.GetUncommentedPlaylistLines(playlist);
            }
        }

        public async Task<byte[]> DownloadData(Uri uri)
        {
            using (WebClient webClient = new WebClient())
            {
                return await webClient.DownloadDataTaskAsync(uri);
            }
        }
    }
}

using System;
using System.Net;

namespace CBDownloader
{
    class Program
    {
        static void Main(string[] args)
        {
            using (WebClient webClient = new WebClient())
            {
                var cbDownloader = new CBDownloader(webClient);
                var playlistUri = new Uri(args[0]);
                cbDownloader.DownloadPlaylist(playlistUri);
                Console.ReadKey();
            }
        }
    }
}

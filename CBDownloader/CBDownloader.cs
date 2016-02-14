using System;
using System.IO;
using System.Net;

namespace CBDownloader
{
    public class CBDownloader : VideoDownloader
    {
        public CBDownloader(WebClient webClient) : base(webClient)
        {
        }

        protected override string DownloadPath(Uri playlistUri)
        {
            return Path.Combine(Directory.GetCurrentDirectory(), "download", DateTime.Now.ToFileTimeUtc().ToString());
        }
    }
}
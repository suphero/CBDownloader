namespace CBDownloader
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            var cbDownloader = new VideoDownloader();
            cbDownloader.DownloadPlaylist().Wait();
        }
    }
}

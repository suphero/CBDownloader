using System;
using System.Collections.Generic;
using System.IO;

namespace CBDownloader
{
    public class MyFileStream
    {
        public void SavePlaylistFile(string path, Uri playlistUri)
        {
            var fileName = DownloaderSettings.Default.PlaylistFileNameToSave;
            using (var file = new StreamWriter(Path.Combine(path, fileName), true))
            {
                file.WriteLine(playlistUri.AbsoluteUri);
                Console.WriteLine(fileName + " saved.");
            }
        }

        public string GetDownloadPath(Uri playlistUri)
        {
			var paths = new List<string> { DownloaderSettings.Default.DownloadPath };
			if (DownloaderSettings.Default.UsePlaylistRegEx)
			{
				paths.Add(GetRegExMainFolder(playlistUri));
			}
			paths.Add(DateTime.Now.ToFileTimeUtc().ToString());
			return Path.Combine(paths.ToArray());
        }

        public void SaveData(byte[] buffer, string path, string fileName)
        {
            using (var fileStream = File.Create(Path.Combine(path, fileName)))
            {
                fileStream.Write(buffer, 0, buffer.Length);
                Console.WriteLine(fileName + " downloaded.");
            }
        }

        public void CreateDirectory(string path)
        {
            Directory.CreateDirectory(path);
        }

        public Uri GetPlaylistUri()
        {
            var file = File.ReadAllText(DownloaderSettings.Default.PlaylistFilePath);
            return new Uri(file);
        }

		string GetRegExMainFolder(Uri playlistUri)
		{
			return Common.GetControlledRegExResult(DownloaderSettings.Default.UsePlaylistRegEx, playlistUri.AbsoluteUri, DownloaderSettings.Default.PlaylistRegExPattern);
		}
    }
}

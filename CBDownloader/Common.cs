using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace CBDownloader
{
    public static class Constraints
    {
        public const string PlaylistFileExtension = ".m3u8";
        public const int RepeatedExceptionCountToBreak = 5;
        public const StringComparison StringComparisonOption = StringComparison.Ordinal;
    }

    public static class Common
    {
        public static List<string> GetUncommentedPlaylistLines(string playlist)
        {
            return playlist.Split('\n')
                           .ToList()
                           .Select(s => s.Trim())
                           .Where(s => !s.StartsWith("#", Constraints.StringComparisonOption))
                           .ToList();
        }


		public static string GetRegExChunk(string chunk)
		{
			var exp = ".*?\\d+.*?(\\d+)";
			var r = new Regex(exp, RegexOptions.IgnoreCase | RegexOptions.Singleline);
			var m = r.Match(chunk);
			if (m.Success)
			{
				return m.Groups[1].ToString();
			}

			throw new Exception("Unable to match with RegEx.");
		}
    }
}

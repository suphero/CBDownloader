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

		public static string GetRegExResult(string input, string pattern)
		{
			var r = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);
			var m = r.Match(input);
			if (m.Success)
			{
				return m.Groups[1].ToString();
			}

			throw new Exception(string.Format("Unable to match {0} with RegEx pattern {1}.", input, pattern));
		}

		public static string GetControlledRegExResult(bool validation, string input, string pattern)
		{
			if (!validation)
			{
				return input;
			}

			return GetRegExResult(input, pattern);
		}
    }
}

using System;
using System.Collections.Generic;
using System.Linq;

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
    }
}

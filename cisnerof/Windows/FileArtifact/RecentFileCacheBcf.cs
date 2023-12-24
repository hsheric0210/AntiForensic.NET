using Serilog;
using System;
using System.IO;

namespace cisnerof.Windows.FileArtifact
{
    /// <summary>
    /// https://www.forensic-cheatsheet.com/Projects/Forensic-Cheatsheet/KR/Artifact/Amcache
    /// </summary>
    internal class RecentFileCacheBcf : ICleaner
    {
        public CleanerTypes Type => CleanerTypes.RecentFileCacheBcf;

        public string Name => "RecentFileCache.bcf";

        public int RunCleaner()
        {
            var file = new FileInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "AppCompat", "Programs", "RecentFileCache.bcf"));
            if (file.Exists)
            {
#if !DEBUG
                file.Delete();
#endif
                Log.Information("Eliminated RecentFileCache.bcf");
                return 1;
            }
            return 0;
        }
    }
}

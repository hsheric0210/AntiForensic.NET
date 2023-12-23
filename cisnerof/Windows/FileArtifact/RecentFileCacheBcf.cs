using Serilog;
using System;
using System.IO;

namespace cisnerof.Windows.FileArtifact
{
    internal class RecentFileCacheBcf : ICleaner
    {
        public string Name => "RecentFileCache.bcf";

        public int RunCleaner()
        {
            var file = new FileInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "AppCompat", "Programs", "RecentFileCache.bcf"));
            if (file.Exists)
            {
                //file.Delete();
                Log.Information("Eliminated RecentFileCache.bcf");
                return 1;
            }
            return 0;
        }
    }
}

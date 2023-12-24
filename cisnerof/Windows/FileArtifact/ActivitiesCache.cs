using Serilog;
using System;
using System.IO;

namespace cisnerof.Windows.FileArtifact
{
    /// <summary>
    /// http://www.forensic-artifacts.com/windows-forensics/timeline
    /// </summary>
    internal class ActivitiesCache : ICleaner
    {
        public CleanerTypes Type => CleanerTypes.ActivitiesCache;

        public string Name => "ConnectedDevicesPlatform ActivitiesCache.db (Timeline)";

        public int RunCleaner()
        {
            var info = new DirectoryInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ConnectedDevicesPlatform"));
            if (!info.Exists)
                return 0;

            var count = 0;
            foreach (var subdir in info.GetDirectories())
            {
                if (!File.Exists(Path.Combine(subdir.FullName, "ActivitiesCache.db")))
                    continue;

                Log.Debug("Found valid subdirectory: {dir}", subdir);
                count += FileUtils.EliminateFolderSubitems(subdir.FullName);
            }

            return count;
        }
    }
}

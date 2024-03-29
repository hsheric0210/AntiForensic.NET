﻿using AntiForensicLib.Properties;
using AntiForensicLib.Windows;
using AntiForensicLib.Windows.FileArtifact;
using AntiForensicLib.Windows.RegistryArtifacts;
using System.Collections.Generic;
using System.IO;

namespace AntiForensicLib
{
    public static class Facade
    {
        public static LoggerProxy Logger { get; set; } = new NullLoggerProxy();

        private static IEnumerable<ICleaner> cleanersCache = null;

        public static IEnumerable<ICleaner> GetCleaners()
        {
            return cleanersCache ?? (cleanersCache = new List<ICleaner>()
            {
                new ActivitiesCache(),
                new AppCompatInstall(),
                new CrashDumps(),
                new IconCache(),
                new PCA(),
                new Prefetch(),
                //new QuickLaunchLnk(),
                new RecentFileCacheBcf(),
                new RecentItems(),
                new RecycleBin(),
                new SRUM(),
                new SearchIndex(),
                //new StartMenuLnk(),
                new WER(),
                new AmCache(),
                new AppCompatCache(),
                new AppCompatFlags(),
                new BAM(),
                new FeatureUsage(),
                new LastVisitedMRU(),
                new MuiCache(),
                new OpenSaveMRU(),
                new RecentDocs(),
                new RunMRU(),
                new Shellbags(),
                new UserAssist(),
                new EventLog(),
                new UsnJrnl(),
            });
        }

        public static void ExtractOffRegLib()
        {
            File.WriteAllBytes("offreg.x86.dll", Resources.offreg_x86);
            File.WriteAllBytes("offreg.x64.dll", Resources.offreg_x64);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cisnerof
{
    [Flags]
    internal enum CleanerTypes : long
    {
        None = 0,
        ActivitiesCache = 1 << 0,
        AppCompatInstall = 1 << 1,
        CrashDumps = 1 << 2,
        IconCache = 1 << 3,
        PCA = 1 << 4,
        Prefetch = 1 << 5,
        QuickLaunchLnk = 1 << 6,
        RecentFileCacheBcf = 1 << 7,
        RecentItems = 1 << 8,
        RecycleBin = 1 << 9,
        SearchIndex = 1 << 10,
        SRUM = 1 << 11,
        StartMenuLnk = 1 << 12,
        WER = 1 << 13,
        Amcache = 1 << 14,
        AppCompatCache = 1 << 15,
        AppCompatFlags = 1 << 16,
        BAM = 1 << 17,
        FeatureUsage = 1 << 18,
        LastVisitedMRU = 1 << 19,
        RecentDocs = 1 << 20,
        RunMRU = 1 << 21,
        Shellbags = 1 << 22,
        UserAssist = 1 << 23,
        EventLog = 1 << 24,
        UsnJrnl = 1 << 25,
        MuiCache = 1 << 26,
        OpenSaveMRU = 1 << 27,
    }
}

using Microsoft.Win32;
using System.IO;

namespace AntiForensicLib.Windows.RegistryArtifacts
{
    internal class LastVisitedMRU : ICleaner
    {
        public CleanerTypes Type => CleanerTypes.LastVisitedMRU;

        public string Name => "Explorer Last-visited MRU";

        public int RunCleaner() => RegUtils.EliminateKeySubentriesRecursive(Registry.CurrentUser.OpenSubKey(Path.Combine("Software", "Microsoft", "Windows", "CurrentVersion", "Explorer", "ComDlg32", "LastVisitedPidlMRU")));
    }
}

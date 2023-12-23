using Microsoft.Win32;
using System.IO;

namespace cisnerof.Windows.RegistryArtifacts
{
    internal class LastVisitedMRU : ICleaner
    {
        public string Name => "Explorer Last-visited MRU";

        public int RunCleaner() => RegUtils.EliminateKeySubentriesRecursive(Registry.CurrentUser.OpenSubKey(Path.Combine("Software", "Microsoft", "Windows", "CurrentVersion", "Explorer", "ComDlg32", "LastVisitedPidlMRU")));
    }
}

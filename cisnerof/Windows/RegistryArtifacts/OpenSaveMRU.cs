using Microsoft.Win32;
using System.IO;

namespace cisnerof.Windows.RegistryArtifacts
{
    /// <summary>
    /// http://www.forensic-artifacts.com/windows-forensics/opensavemru
    /// </summary>
    internal class OpenSaveMRU : ICleaner
    {
        public CleanerTypes Type => CleanerTypes.OpenSaveMRU;

        public string Name => "Explorer Open/Save MRU";

        public int RunCleaner() => RegUtils.EliminateKeySubentriesRecursive(Registry.CurrentUser.OpenSubKey(Path.Combine("Software", "Microsoft", "Windows", "CurrentVersion", "Explorer", "ComDlg32", "OpenSavePidlMRU")));
    }
}

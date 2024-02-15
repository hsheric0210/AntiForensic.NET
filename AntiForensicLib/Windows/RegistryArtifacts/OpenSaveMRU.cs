using Microsoft.Win32;
using System.IO;

namespace AntiForensicLib.Windows.RegistryArtifacts
{
    /// <summary>
    /// http://www.forensic-artifacts.com/windows-forensics/opensavemru
    /// </summary>
    internal class OpenSaveMRU : ICleaner
    {
        public CleanerTypes Type => CleanerTypes.OpenSaveMRU;

        public string Name => "Explorer Open/Save MRU";

        public int RunCleaner() => RegistryUtils.EliminateKeySubentriesRecursive(Registry.CurrentUser.OpenSubKey(Path.Combine("Software", "Microsoft", "Windows", "CurrentVersion", "Explorer", "ComDlg32", "OpenSavePidlMRU")));
    }
}

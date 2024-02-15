using Microsoft.Win32;
using System.IO;

namespace AntiForensicLib.Windows.RegistryArtifacts
{
    /// <summary>
    /// http://www.forensic-artifacts.com/windows-forensics/recentfiles
    /// </summary>
    internal class RecentDocs : ICleaner
    {
        public CleanerTypes Type => CleanerTypes.RecentDocs;

        public string Name => "Explorer Recent Docs";

        public int RunCleaner() => RegUtils.EliminateKeySubentriesRecursive(Registry.CurrentUser.OpenSubKey(Path.Combine("Software", "Microsoft", "Windows", "CurrentVersion", "Explorer", "RecentDocs")));
    }
}

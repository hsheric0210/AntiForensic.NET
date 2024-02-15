using Microsoft.Win32;
using System.IO;

namespace AntiForensicLib.Windows.RegistryArtifacts
{
    /// <summary>
    /// http://www.forensic-artifacts.com/windows-forensics/muicache
    /// </summary>
    internal class MuiCache : ICleaner
    {
        public CleanerTypes Type => CleanerTypes.MuiCache;

        public string Name => "MuiCache";

        public int RunCleaner()
        {
            var count = 0;
            count += RegUtils.EliminateKeySubentriesRecursive(Registry.CurrentUser.OpenSubKey(Path.Combine("Software", "Classes", "Local Settings", "MuiCache")));
            count += RegUtils.EliminateKeySubentriesRecursive(Registry.CurrentUser.OpenSubKey(Path.Combine("Software", "Classes", "Local Settings", "Software", "Microsoft", "Windows", "Shell", "MuiCache")));
            return count;
        }
    }
}

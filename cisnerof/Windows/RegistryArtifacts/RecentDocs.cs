using Microsoft.Win32;
using System.IO;

namespace cisnerof.Windows.RegistryArtifacts
{
    internal class RecentDocs : ICleaner
    {
        public string Name => "Explorer Recent Docs";

        public int RunCleaner() => RegUtils.EliminateKeySubentriesRecursive(Registry.CurrentUser.OpenSubKey(Path.Combine("Software", "Microsoft", "Windows", "CurrentVersion", "Explorer", "RecentDocs")));
    }
}

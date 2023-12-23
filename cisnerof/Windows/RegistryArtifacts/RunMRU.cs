using Microsoft.Win32;
using System.IO;
using System.Security.Principal;

namespace cisnerof.Windows.RegistryArtifacts
{
    internal class RunMRU : ICleaner
    {
        public string Name => "Run(Ctrl+R) MRU";

        public int RunCleaner() => RegUtils.EliminateKeySubentriesRecursive(Registry.CurrentUser.OpenSubKey(Path.Combine("Software", "Microsoft", "Windows", "CurrentVersion", "Explorer", "RunMRU")));
    }
}

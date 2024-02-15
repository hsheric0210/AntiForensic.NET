using Microsoft.Win32;
using System.IO;

namespace AntiForensicLib.Windows.RegistryArtifacts
{
    internal class RunMRU : ICleaner
    {
        public CleanerTypes Type => CleanerTypes.RunMRU;

        public string Name => "Run(Ctrl+R) MRU";

        public int RunCleaner() => RegistryUtils.EliminateKeySubentriesRecursive(Registry.CurrentUser.OpenSubKey(Path.Combine("Software", "Microsoft", "Windows", "CurrentVersion", "Explorer", "RunMRU")));
    }
}

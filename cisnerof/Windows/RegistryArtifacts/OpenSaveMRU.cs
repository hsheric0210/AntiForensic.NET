using Microsoft.Win32;
using System.IO;

namespace cisnerof.Windows.RegistryArtifacts
{
    internal class OpenSaveMRU : ICleaner
    {
        public string Name => "Explorer Open/Save MRU";

        public int RunCleaner()
        {
            var count = 0;
            count += RegUtils.EliminateKeySubentriesRecursive(Registry.CurrentUser.OpenSubKey(Path.Combine("Software", "Classes", "Local Settings", "MuiCache")));
            count += RegUtils.EliminateKeySubentriesRecursive(Registry.CurrentUser.OpenSubKey(Path.Combine("Software", "Classes", "Local Settings", "Software", "Microsoft", "Windows", "Shell", "MuiCache")));
            return count;
        }
    }
}

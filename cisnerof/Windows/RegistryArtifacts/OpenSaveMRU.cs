using Microsoft.Win32;
using System.IO;

namespace cisnerof.Windows.RegistryArtifacts
{
    internal class OpenSaveMRU : ICleaner
    {
        public string Name => "Explorer Open/Save MRU";

        public int RunCleaner() => RegUtils.EliminateKeySubentriesRecursive(Registry.CurrentUser.OpenSubKey(Path.Combine("Software", "Microsoft", "Windows", "CurrentVersion", "Explorer", "ComDlg32", "OpenSavePidlMRU")));
    }
}

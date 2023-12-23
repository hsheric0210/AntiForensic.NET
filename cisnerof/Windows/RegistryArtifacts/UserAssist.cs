using Microsoft.Win32;
using System.IO;

namespace cisnerof.Windows.RegistryArtifacts
{
    internal class UserAssist : ICleaner
    {
        public string Name => "Explorer UserAssist";

        public int RunCleaner()
        {
            var count = 0;
            var key = Registry.CurrentUser.OpenSubKey(Path.Combine("Software", "Microsoft", "Windows", "CurrentVersion", "Explorer", "UserAssist"));
            foreach (var subkey in key.GetSubKeyNames())
                count += RegUtils.EliminateKeySubentriesRecursive(key.OpenSubKey(Path.Combine(subkey, "Count")));

            return count;
        }
    }
}

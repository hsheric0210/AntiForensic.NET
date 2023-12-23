using Microsoft.Win32;
using System.IO;

namespace cisnerof.Windows.RegistryArtifacts
{
    internal class Shellbags : ICleaner
    {
        public string Name => "Explorer Shellbags";

        public int RunCleaner()
        {
            var count = 0;
            count += RegUtils.EliminateKeySubentriesRecursive(Registry.CurrentUser.OpenSubKey(Path.Combine("Software", "Classes", "Local Settings", "Software", "Microsoft", "Windows", "Shell", "Bags")));
            count += RegUtils.EliminateKeySubentriesRecursive(Registry.CurrentUser.OpenSubKey(Path.Combine("Software", "Classes", "Local Settings", "Software", "Microsoft", "Windows", "Shell", "BagMRU")));
            count += RegUtils.EliminateKeySubentriesRecursive(Registry.CurrentUser.OpenSubKey(Path.Combine("Software", "Classes", "WoW6432Node", "Local Settings", "Software", "Microsoft", "Windows", "Shell", "Bags")));
            count += RegUtils.EliminateKeySubentriesRecursive(Registry.CurrentUser.OpenSubKey(Path.Combine("Software", "Classes", "WoW6432Node", "Local Settings", "Software", "Microsoft", "Windows", "Shell", "BagMRU")));
            count += RegUtils.EliminateKeySubentriesRecursive(Registry.CurrentUser.OpenSubKey(Path.Combine("Software", "Microsoft", "Windows", "Shell", "Bags")));
            count += RegUtils.EliminateKeySubentriesRecursive(Registry.CurrentUser.OpenSubKey(Path.Combine("Software", "Microsoft", "Windows", "Shell", "BagMRU")));

            PutDefaultMRU(Registry.CurrentUser.OpenSubKey(Path.Combine("Software", "Classes", "Local Settings", "Software", "Microsoft", "Windows", "Shell", "BagMRU")));
            PutDefaultMRU(Registry.CurrentUser.OpenSubKey(Path.Combine("Software", "Classes", "WoW6432Node", "Local Settings", "Software", "Microsoft", "Windows", "Shell", "BagMRU")));
            PutDefaultMRU(Registry.CurrentUser.OpenSubKey(Path.Combine("Software", "Microsoft", "Windows", "Shell", "BagMRU")));

            return count;
        }

        private void PutDefaultMRU(RegistryKey key)
        {
            if (key == null)
                return;

#if !DEBUG
            key.SetValue("1", new byte[] { 0xAA, 0xBB, 0xCC, 0, 11, 0x22, 0x33 });
#endif
        }
    }
}

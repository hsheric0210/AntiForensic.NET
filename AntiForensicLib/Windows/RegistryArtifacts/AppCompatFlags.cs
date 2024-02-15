using Microsoft.Win32;
using System.IO;

namespace AntiForensicLib.Windows.RegistryArtifacts
{
    /// <summary>
    /// https://www.sevenforums.com/general-discussion/384201-appcompatcache-registry-windows-7-a.html
    /// </summary>
    internal class AppCompatFlags : ICleaner
    {
        public CleanerTypes Type => CleanerTypes.AppCompatFlags;

        public string Name => "Application Compatibility Flags (AppCompatFlags) Store";

        public int RunCleaner() => RegistryUtils.EliminateKeySubentriesRecursive(Registry.CurrentUser.OpenSubKey(Path.Combine("Software", "Microsoft", "Windows NT", "CurrentVersion", "AppCompatFlags", "Compatibility Assistant", "Store")));
    }
}

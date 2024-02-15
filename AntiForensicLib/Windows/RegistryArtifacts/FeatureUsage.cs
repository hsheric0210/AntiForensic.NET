using Microsoft.Win32;
using System.IO;

namespace AntiForensicLib.Windows.RegistryArtifacts
{
    /// <summary>
    /// http://www.forensic-artifacts.com/windows-forensics/userassist
    /// </summary>
    internal class FeatureUsage : ICleaner
    {
        public CleanerTypes Type => CleanerTypes.FeatureUsage;

        public string Name => "Explorer FeatureUsage";

        public int RunCleaner()
        {
            var count = 0;
            var featureUsage = Registry.CurrentUser.OpenSubKey(Path.Combine("Software", "Microsoft", "Windows", "CurrentVersion", "Explorer", "FeatureUsage"));
            count += RegUtils.EliminateKeySubentries(featureUsage.OpenSubKey("AppBadgeUpdated"));
            count += RegUtils.EliminateKeySubentries(featureUsage.OpenSubKey("AppLaunch"));
            count += RegUtils.EliminateKeySubentries(featureUsage.OpenSubKey("AppSwitched"));
            count += RegUtils.EliminateKeySubentries(featureUsage.OpenSubKey("ShowJumpView"));
            return count;
        }
    }
}

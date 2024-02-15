using AntiForensicLib;
using Microsoft.Win32;
using System.IO;

namespace AntiForensicLib.Windows.RegistryArtifacts
{
    /// <summary>
    /// http://www.forensic-artifacts.com/windows-forensics/bamdam
    /// </summary>
    internal class BAM : ICleaner
    {
        public CleanerTypes Type => CleanerTypes.BAM;

        public string Name => "Background Activity Moderator";

        public int RunCleaner()
        {
            var count = 0;
            var key = Registry.LocalMachine.OpenSubKey(Path.Combine("SYSTEM", "CurrentControlSet", "Services", "bam", "State", "UserSettings"));
            foreach (var subkey in key.GetSubKeyNames())
            {
                Facade.Logger.Debug(string.Format("Found valid bam UserSettings SID subkey: {0}", subkey));
                count += RegUtils.EliminateKeySubentries(key.OpenSubKey(subkey), FilterFunc);
            }

            return count;
        }

        private bool FilterFunc(string name) => "SequenceNumber".Equals(name) || "Version".Equals(name);
    }
}

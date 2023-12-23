using Microsoft.Win32;
using System.IO;

namespace cisnerof.Windows.RegistryArtifacts
{
    internal class BAM : ICleaner
    {
        public string Name => "Background Activity Moderator";

        public int RunCleaner()
        {
            var count = 0;
            var key = Registry.LocalMachine.OpenSubKey(Path.Combine("SYSTEM", "CurrentControlSet", "Services", "bam", "State", "UserSettings"));
            foreach (var subkey in key.GetSubKeyNames())
                count += RegUtils.EliminateKeySubentries(key.OpenSubKey(subkey), FilterFunc);

            return count;
        }

        private bool FilterFunc(string name) => "SequenceNumber".Equals(name) || "Version".Equals(name);
    }
}

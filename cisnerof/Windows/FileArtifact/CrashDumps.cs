using System;
using System.IO;

namespace cisnerof.Windows.FileArtifact
{
    internal class CrashDumps : ICleaner
    {
        public string Name => "Crash Dumps";

        public int RunCleaner() => FileUtils.EliminateFolderSubitems(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CrashDumps"));
    }
}

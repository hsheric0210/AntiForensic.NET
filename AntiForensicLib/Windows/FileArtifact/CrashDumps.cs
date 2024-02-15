using System;
using System.IO;

namespace AntiForensicLib.Windows.FileArtifact
{
    internal class CrashDumps : ICleaner
    {
        public CleanerTypes Type => CleanerTypes.CrashDumps;

        public string Name => "Crash Dumps";

        public int RunCleaner() => FileUtils.EliminateFolderSubitems(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CrashDumps"));
    }
}

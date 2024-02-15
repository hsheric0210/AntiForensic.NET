using System;
using System.IO;

namespace AntiForensicLib.Windows.FileArtifact
{
    /// <summary>
    /// https://www.forensic-cheatsheet.com/Projects/Forensic-Cheatsheet/KR/Artifact/Prefetch+%26+Superfetch
    /// </summary>
    internal class Prefetch : ICleaner
    {
        public CleanerTypes Type => CleanerTypes.Prefetch;

        public string Name => "Windows Prefetch, Superfetch Files";

        public int RunCleaner()
        {
            var count = FileUtils.EliminateFolderSubitems(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "Prefetch"), "*.pf")
                + FileUtils.EliminateFolderSubitems(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "Prefetch"), "*.db");

            var layoutIni = new FileInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "Prefetch", "Layout.ini"));
            if (layoutIni.Exists)
            {
#if !DEBUG
                layoutIni.Delete();
#endif
                Facade.Logger.Information("Deleted prefetch layout.ini");
                count++;
            }

            return count;
        }
    }
}

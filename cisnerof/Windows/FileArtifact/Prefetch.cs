using Serilog;
using System;
using System.IO;

namespace cisnerof.Windows.FileArtifact
{
    internal class Prefetch : ICleaner
    {
        public string Name => "Windows Prefetch Files";

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
                Log.Debug("Deleted prefetch layout.ini");
                count++;
            }

            return count;
        }
    }
}

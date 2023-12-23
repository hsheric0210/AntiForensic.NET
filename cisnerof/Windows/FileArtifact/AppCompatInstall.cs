using System;
using System.IO;

namespace cisnerof.Windows.FileArtifact
{
    internal class AppCompatInstall : ICleaner
    {
        public string Name => "AppCompat Installation Logs";

        public int RunCleaner() => FileUtils.EliminateFolderSubitems(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "AppCompat", "Programs", "Install"), "*.txt");
    }
}

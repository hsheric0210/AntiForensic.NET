using System;
using System.IO;

namespace cisnerof.Windows.FileArtifact
{
    internal class PCA : ICleaner
    {
        public string Name => "AppCompat PCA";

        public int RunCleaner() => FileUtils.EliminateFolderSubitems(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "AppCompat", "PCA"), "*.txt");
    }
}

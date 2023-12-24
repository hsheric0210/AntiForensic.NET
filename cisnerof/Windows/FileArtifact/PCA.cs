using System;
using System.IO;

namespace cisnerof.Windows.FileArtifact
{
    /// <summary>
    /// https://www.forensic-cheatsheet.com/Projects/Forensic-Cheatsheet/KR/Artifact/PCA
    /// </summary>
    internal class PCA : ICleaner
    {
        public CleanerTypes Type => CleanerTypes.PCA;

        public string Name => "Program Compatibility Assistant";

        public int RunCleaner() => FileUtils.EliminateFolderSubitems(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "AppCompat", "PCA"), "*.txt");
    }
}

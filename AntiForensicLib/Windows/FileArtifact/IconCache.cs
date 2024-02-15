using System;
using System.IO;

namespace AntiForensicLib.Windows.FileArtifact
{
    /// <summary>
    /// https://www.forensic-cheatsheet.com/Projects/Forensic-Cheatsheet/KR/Artifact/ThumbCache++IconCache
    /// </summary>
    internal class IconCache : ICleaner
    {
        public CleanerTypes Type => CleanerTypes.IconCache;

        public string Name => "Exploer Icon Cache";

        public int RunCleaner()
        {
            return FileUtils.EliminateSingleFile(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "IconCache.db"))
                + FileUtils.EliminateFolderSubitems(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Microsoft", "Windows", "Explorer"), "*.db");
        }
    }
}

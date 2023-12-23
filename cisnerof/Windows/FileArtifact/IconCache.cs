using System;
using System.IO;

namespace cisnerof.Windows.FileArtifact
{
    internal class IconCache : ICleaner
    {
        public string Name => "Exploer Icon Cache";

        public int RunCleaner()
        {
            return FileUtils.EliminateSingleFile(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "IconCache.db"))
                + FileUtils.EliminateFolderSubitems(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Microsoft", "Windows", "Explorer"), "*.db");
        }
    }
}

using System;

namespace cisnerof.Windows.FileArtifact
{
    internal class RecentItems : ICleaner
    {
        public string Name => "Explorer Recent Items";

        public int RunCleaner() => FileUtils.EliminateFolderSubitems(Environment.GetFolderPath(Environment.SpecialFolder.Recent));
    }
}

using System;

namespace AntiForensicLib.Windows.FileArtifact
{
    /// <summary>
    /// http://www.forensic-artifacts.com/windows-forensics/jumplist
    /// </summary>
    internal class RecentItems : ICleaner
    {
        public CleanerTypes Type => CleanerTypes.RecentItems;

        public string Name => "Explorer Recent Items";

        public int RunCleaner() => FileUtils.EliminateFolderSubitems(Environment.GetFolderPath(Environment.SpecialFolder.Recent));
    }
}

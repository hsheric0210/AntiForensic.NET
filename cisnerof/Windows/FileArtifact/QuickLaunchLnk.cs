using System;
using System.IO;
using System.Linq;

namespace cisnerof.Windows.FileArtifact
{
    internal class QuickLaunchLnk : ICleaner
    {
        private static readonly string[] blacklistedNames = new string[] {
            "Window Switcher.lnk", // 창 간 전환
            "Shows Desktop.lnk", // 바탕 화면 보기
            "File Explorer.lnk", // Windows 탐색기
        };

        public string Name => "Quick Launch lnk";

        public int RunCleaner() => FileUtils.EliminateFolderSubitems(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Microsoft", "Internet Explorer", "Quick Launch"));

        public bool FilterFunc(FileSystemInfo info)
        {
            if (info is FileInfo)
                return !blacklistedNames.Any(name => name.Equals(info.Name, StringComparison.OrdinalIgnoreCase));

            return false;
        }
    }
}

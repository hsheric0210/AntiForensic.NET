using System;
using System.IO;
using System.Linq;

namespace cisnerof.Windows.FileArtifact
{
    /// <summary>
    /// http://www.forensic-artifacts.com/windows-forensics/linkfile
    /// </summary>
    internal class StartMenuLnk : ICleaner
    {
        public CleanerTypes Type => CleanerTypes.StartMenuLnk;

        private static readonly string[] blacklistedFolders = new string[] {
            "Accessibility", // Windows 접근성
            "Accessories", // Windows 보조프로그램
            "Administrative Tools", // Windows 관리 도구
            "Startup", // 시작프로그램
            "System Tools", // Windows 시스템
        };

        public string Name => "Explorer Start Menu Lnk";

        public int RunCleaner() => FileUtils.EliminateFolderSubitems(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Microsoft", "Windows", "Start Menu"), FilterFunc);

        public bool FilterFunc(FileSystemInfo info)
        {
            if (info is FileInfo)
            {
                if (!"lnk".Equals(info.Extension, StringComparison.OrdinalIgnoreCase))
                    return false; // Not a lnk file

                if (blacklistedFolders.Any(name => name.Equals(Path.GetDirectoryName(info.FullName), StringComparison.OrdinalIgnoreCase)))
                    return false;
            }
            else if (info is DirectoryInfo && blacklistedFolders.Any(name => name.Equals(info.Name, StringComparison.OrdinalIgnoreCase)))
            {
                return false;
            }

            return true;
        }
    }
}

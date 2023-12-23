using System;
using System.IO;

namespace cisnerof.Windows.FileArtifact
{
    internal class RecycleBin : ICleaner
    {
        public string Name => "Empty Recycle Bin";

        public int RunCleaner()
        {
            var count = 0;
            foreach (var disk in DriveInfo.GetDrives())
            {
                if (!disk.IsReady || disk.DriveType != DriveType.Fixed && disk.DriveType != DriveType.Removable)
                    continue;

                var bin = new DirectoryInfo(Path.Combine(disk.RootDirectory.FullName, "$Recycle.Bin"));
                if (bin.Exists)
                {
                    foreach (var subdir in bin.EnumerateDirectories())
                        FileUtils.EliminateFolderSubitems(subdir.FullName, DesktopIniFilter);
                }
            }
            return count;
        }

        private bool DesktopIniFilter(FileSystemInfo path) => !"desktop.ini".Equals(path.Name, StringComparison.OrdinalIgnoreCase);
    }
}

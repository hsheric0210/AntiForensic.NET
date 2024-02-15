using AntiForensicLib;
using System;
using System.IO;

namespace AntiForensicLib.Windows.FileArtifact
{
    /// <summary>
    /// https://www.forensic-cheatsheet.com/Projects/Forensic-Cheatsheet/KR/Artifact/Recycle+Bin
    /// </summary>
    internal class RecycleBin : ICleaner
    {
        public CleanerTypes Type => CleanerTypes.RecycleBin;

        public string Name => "Empty Recycle Bin";

        public int RunCleaner()
        {
            var count = 0;
            foreach (var disk in DriveInfo.GetDrives())
            {
                if (!disk.IsReady || disk.DriveType != DriveType.Fixed && disk.DriveType != DriveType.Removable)
                    continue;

                Facade.Logger.Debug("Found valid drive: " + disk.Name);

                var bin = new DirectoryInfo(Path.Combine(disk.RootDirectory.FullName, "$Recycle.Bin"));
                if (bin.Exists)
                {
                    Facade.Logger.Debug("Found valid recycle bin: " + bin.FullName);

                    try
                    {
                        foreach (var subdir in bin.EnumerateDirectories())
                            FileUtils.EliminateFolderSubitems(subdir.FullName, DesktopIniFilter);
                    }
                    catch (Exception ex)
                    {
                        Facade.Logger.Error(ex, string.Format("Error deleting recycle bin: {0}", bin.FullName));
                    }
                }
            }

            return count;
        }

        private bool DesktopIniFilter(FileSystemInfo path) => !"desktop.ini".Equals(path.Name, StringComparison.OrdinalIgnoreCase);
    }
}

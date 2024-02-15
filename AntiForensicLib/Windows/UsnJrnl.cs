using System.Diagnostics;
using System.IO;

namespace AntiForensicLib.Windows
{
    internal class UsnJrnl : ICleaner
    {
        public CleanerTypes Type => CleanerTypes.UsnJrnl;

        public string Name => "Usn Journal Cleaner";

        public int RunCleaner()
        {
            var count = 0;
            foreach (var drive in DriveInfo.GetDrives())
            {
                if (!drive.IsReady || drive.DriveType != DriveType.Fixed && drive.DriveType != DriveType.Removable)
                    continue;

                if (drive.DriveFormat != "NTFS")
                    continue; // Only NTFS supports UsnJrnl

                var procinfo = new ProcessStartInfo();
                procinfo.FileName = "fsutil";
                procinfo.Arguments = "usn deleteJournal /D " + drive.RootDirectory.FullName.TrimEnd(Path.DirectorySeparatorChar);
#if DEBUG
                Facade.Logger.Information("Simulated fsutil execution: " + procinfo.Arguments);
#else
                Process.Start(procinfo).WaitForExit();
#endif
                Facade.Logger.Debug(string.Format("Cleared UsnJrnl of {0}", drive.RootDirectory));
                count++;
            }

            return count;
        }
    }
}

using Serilog;
using System.Diagnostics;
using System.IO;

namespace cisnerof.Windows
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
                Log.Information("fsutil execute: " + procinfo.Arguments);
#else
                Process.Start(procinfo).WaitForExit();
#endif
                Log.Debug("Cleared UsnJrnl of {drive}", drive.RootDirectory);
                count++;
            }

            return count;
        }
    }
}

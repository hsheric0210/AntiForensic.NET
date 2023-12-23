using System;
using System.Diagnostics;
using System.IO;

namespace cisnerof.Windows.FileArtifact
{
    internal class SRUM : ICleaner
    {
        public string Name => "SRU Monitor";

        public int RunCleaner()
        {
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "System32", "SRU", "SRUDB.dat");
            if (!File.Exists(path))
                return 0;

            var process = new ProcessStartInfo();
            process.FileName = "sc.exe";
            process.Arguments = "stop DPS"; // Diagnostic Policy Service
            process.WindowStyle = ProcessWindowStyle.Hidden;
            var proc = Process.Start(process);
            proc.WaitForExit();

            File.Delete(path);

            process = new ProcessStartInfo();
            process.FileName = "sc.exe";
            process.Arguments = "start DPS"; // Diagnostic Policy Service
            process.WindowStyle = ProcessWindowStyle.Hidden;
            proc = Process.Start(process);
            proc.WaitForExit();

            return 1;
        }
    }
}

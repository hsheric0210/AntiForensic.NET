using Serilog;
using System;
using System.Diagnostics;
using System.IO;

namespace cisnerof.Windows.FileArtifact
{
    /// <summary>
    /// https://www.forensic-cheatsheet.com/Projects/Forensic-Cheatsheet/KR/Artifact/SRUM
    /// </summary>
    internal class SRUM : ICleaner
    {
        public CleanerTypes Type => CleanerTypes.SRUM;

        public string Name => "System Resource Utilization Monitor";

        public int RunCleaner()
        {
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "System32", "SRU", "SRUDB.dat");
            if (!File.Exists(path))
                return 0;

            ServiceUtils.StopService("DPS");

#if !DEBUG
            File.Delete(path);
#endif
            Log.Information("Eliminated SRU DB {path}", path);

            ServiceUtils.StartService("DPS");
            return 1;
        }
    }
}

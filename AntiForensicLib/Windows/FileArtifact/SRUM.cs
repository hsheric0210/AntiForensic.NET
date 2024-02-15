using AntiForensicLib;
using System;
using System.IO;

namespace AntiForensicLib.Windows.FileArtifact
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
            Facade.Logger.Information(string.Format("Eliminated SRU DB: {0}", path));

            ServiceUtils.StartService("DPS");
            return 1;
        }
    }
}

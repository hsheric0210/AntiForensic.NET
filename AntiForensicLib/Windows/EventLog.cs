using AntiForensicLib;
using System.Diagnostics;

namespace AntiForensicLib.Windows
{
    internal class EventLog : ICleaner
    {
        public CleanerTypes Type => CleanerTypes.EventLog;

        public string Name => "Event Log";

        public int RunCleaner()
        {
            var procinfo = new ProcessStartInfo
            {
                FileName = "WEVTUTIL.EXE",
                Arguments = "ENUM-LOGS",
                RedirectStandardOutput = true,
                UseShellExecute = false
            };
            var proc = Process.Start(procinfo);
            var logs = proc.StandardOutput.ReadToEnd().Split('\n');
            proc.WaitForExit();

            foreach (var name in logs)
            {
                var logname = name.TrimEnd('\r', '\n');
                if (string.IsNullOrWhiteSpace(logname))
                    continue;

                Facade.Logger.Debug(string.Format("Clearing event log: {0}", logname));

                var info = new ProcessStartInfo
                {
                    FileName = "WEVTUTIL.EXE",
                    Arguments = "CLEAR-LOG \"" + logname + '\"'
                };

#if !DEBUG
                var proc2 = Process.Start(info); // no waiting. it will execute in parallel
                proc2.WaitForExit();
                Facade.Logger.Debug(string.Format("Event log cleaner for {log} exit: {code}", proc2.ExitCode));
#endif
            }

            return logs.Length;
        }
    }
}

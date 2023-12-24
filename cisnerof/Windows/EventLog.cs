using Serilog;
using System.Diagnostics;

namespace cisnerof.Windows
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

                Log.Debug("Clearing event log: {log}", logname);

                var info = new ProcessStartInfo
                {
                    FileName = "WEVTUTIL.EXE",
                    Arguments = "CLEAR-LOG \"" + logname + '\"'
                };

#if !DEBUG
                var proc2 = Process.Start(info); // no waiting. it will execute in parallel
                proc2.WaitForExit();
                Log.Debug("Event log cleaner for {log} exit: {code}", proc2.ExitCode);
#endif
            }

            return logs.Length;
        }
    }
}

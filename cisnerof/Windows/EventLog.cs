using Serilog;
using System.Diagnostics;

namespace cisnerof.Windows
{
    internal class EventLog : ICleaner
    {
        public string Name => "Event Log";

        public int RunCleaner()
        {
            var procinfo = new ProcessStartInfo();
            procinfo.FileName = "WEVTUTIL.EXE";
            procinfo.Arguments = "ENUM-LOGS";
            procinfo.RedirectStandardOutput = true;
            procinfo.UseShellExecute = false;
            var proc = Process.Start(procinfo);
            var logs = proc.StandardOutput.ReadToEnd().Split('\n');
            proc.WaitForExit();

            foreach (var name in logs)
            {
                var logname = name.TrimEnd('\r', '\n');
                if (string.IsNullOrWhiteSpace(logname))
                    continue;

                Log.Debug("Clearing event log: {log}", logname);

                var info = new ProcessStartInfo();
                info.FileName = "WEVTUTIL.EXE";
                info.Arguments = "CLEAR-LOG \"" + logname + '\"';

#if !DEBUG
                Process.Start(info); // no waiting. it will execute in parallel
#endif
            }

            return logs.Length;
        }
    }
}

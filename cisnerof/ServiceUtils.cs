using Serilog;
using System.Diagnostics;

namespace cisnerof
{
    internal static class ServiceUtils
    {
        public static void StartService(string serviceName)
        {
            var info = new ProcessStartInfo
            {
                FileName = "sc.exe",
                Arguments = "start " + serviceName,
                WindowStyle = ProcessWindowStyle.Hidden
            };

            Log.Debug("Service Controller execute: {param}", info.Arguments);
            var proc = Process.Start(info);
            proc.WaitForExit();

            Log.Debug("Service Controller exit: {code}", proc.ExitCode);
        }

        public static void StopService(string serviceName)
        {
            var info = new ProcessStartInfo
            {
                FileName = "sc.exe",
                Arguments = "stop " + serviceName,
                WindowStyle = ProcessWindowStyle.Hidden
            };

            Log.Debug("Service Controller execute: {param}", info.Arguments);
            var proc = Process.Start(info);
            proc.WaitForExit();

            Log.Debug("Service Controller exit: {code}", proc.ExitCode);
        }
    }
}

using AntiForensicLib;
using Serilog;
using System;
using System.Globalization;
using System.IO;

namespace AntiForensicTool
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0 || !long.TryParse(args[0], NumberStyles.AllowHexSpecifier, NumberFormatInfo.InvariantInfo, out var flags))
                flags = long.MaxValue & ~(long)CleanerTypes.QuickLaunchLnk & ~(long)CleanerTypes.StartMenuLnk; // disabled by default

            Facade.ExtractOffRegLib();

            var logFile = Path.GetRandomFileName() + '.' + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss_ffff") + ".log";
            Console.WriteLine("Log: " + logFile);
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.File(logFile, buffered: true)
                .CreateLogger();

            try
            {
                Facade.Logger = new SerilogProxy();

                var list = Facade.GetCleaners();

                foreach (var job in list)
                {
                    if (!((CleanerTypes)flags).HasFlag(job.Type))
                    {
                        Log.Information("Skip running: {name}", job.Name);
                        continue;
                    }

                    Log.Information("Start cleaner: {name}", job.Name);
                    try
                    {
                        job.RunCleaner();
                        Log.Information("End cleaner: {name}", job.Name);
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex, "Error on cleaner: {name}", job.Name);
                    }
                }
            }
            finally
            {
                Console.WriteLine("Done");
                Log.CloseAndFlush();
            }
        }
    }
}

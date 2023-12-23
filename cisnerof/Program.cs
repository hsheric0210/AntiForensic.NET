using cisnerof.Properties;
using cisnerof.Windows;
using cisnerof.Windows.FileArtifact;
using cisnerof.Windows.RegistryArtifacts;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using System;
using System.Collections.Generic;
using System.IO;

namespace cisnerof
{
    internal class Program
    {
        static void Main(string[] _)
        {
            File.WriteAllBytes("offreg.x86.dll", Resources.offreg_x86);
            File.WriteAllBytes("offreg.x64.dll", Resources.offreg_x64);

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.File("job.log", buffered: true)
                .WriteTo.Console(Serilog.Events.LogEventLevel.Information, theme: AnsiConsoleTheme.Code)
                .CreateLogger();

            try
            {
                var list = new List<ICleaner>()
                {
                    new ActivitiesCache(),
                    new AppCompatInstall(),
                    new CrashDumps(),
                    new IconCache(),
                    new PCA(),
                    new Prefetch(),
                    new QuickLaunchLnk(),
                    new RecentFileCacheBcf(),
                    new RecentItems(),
                    new RecycleBin(),
                    new SRUM(),
                    new StartMenuLnk(),
                    new AmCache(),
                    new AppCompatCache(),
                    new BAM(),
                    new OpenSaveMRU(),
                    new LastVisitedMRU(),
                    new MuiCache(),
                    new RecentDocs(),
                    new RunMRU(),
                    new Shellbags(),
                    new UserAssist(),
                    new UsnJrnl(),
                    new EventLog()
                };

                foreach (var job in list)
                {
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
                FileUtils.EliminateSingleFile("offreg.x86.dll");
                FileUtils.EliminateSingleFile("offreg.x64.dll");
                Log.CloseAndFlush();
            }
        }
    }
}

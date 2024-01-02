using cisnerof.Properties;
using cisnerof.Windows;
using cisnerof.Windows.FileArtifact;
using cisnerof.Windows.RegistryArtifacts;
using Serilog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace cisnerof
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0 || !long.TryParse(args[0], NumberStyles.AllowHexSpecifier, NumberFormatInfo.InvariantInfo, out var flags))
                flags = long.MaxValue & ~(long)CleanerTypes.QuickLaunchLnk & ~(long)CleanerTypes.StartMenuLnk; // disabled by default

            File.WriteAllBytes("offreg.x86.dll", Resources.offreg_x86);
            File.WriteAllBytes("offreg.x64.dll", Resources.offreg_x64);

            var logFile = Path.GetRandomFileName() + '.' + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss_ffff") + ".log";
            Console.WriteLine("Log: " + logFile);
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.File(logFile, buffered: true)
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
                    //new QuickLaunchLnk(),
                    new RecentFileCacheBcf(),
                    new RecentItems(),
                    new RecycleBin(),
                    new SRUM(),
                    new SearchIndex(),
                    //new StartMenuLnk(),
                    new WER(),
                    new AmCache(),
                    new AppCompatCache(),
                    new AppCompatFlags(),
                    new BAM(),
                    new FeatureUsage(),
                    new LastVisitedMRU(),
                    new MuiCache(),
                    new OpenSaveMRU(),
                    new RecentDocs(),
                    new RunMRU(),
                    new Shellbags(),
                    new UserAssist(),
                    new EventLog(),
                    new UsnJrnl(),
                };

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
                FileUtils.EliminateSingleFile("offreg.x86.dll");
                FileUtils.EliminateSingleFile("offreg.x64.dll");
                Log.CloseAndFlush();
            }
        }
    }
}

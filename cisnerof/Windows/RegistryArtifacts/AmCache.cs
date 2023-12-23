using Serilog;
using System;
using System.IO;
using OffregLib;

namespace cisnerof.Windows.RegistryArtifacts
{
    /// <summary>
    /// Mount AmCache.hve, Delete all subkeys of 'InventoryApplication*' keys, Unmount.
    /// </summary>
    internal class AmCache : ICleaner
    {
        public string Name => "Amcache.hve";

        public int RunCleaner()
        {
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "AppCompat", "Programs", "AmCache.hve");
            if (!File.Exists(path))
                return 0;

            var copyName = Path.GetRandomFileName();
            try
            {
                File.Copy(path, copyName);
                Log.Information("Amcache hive copied to: {path}", copyName);

#if !DEBUG
                File.Delete(path);
#endif
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error copying & deleting Amcache.hve file to random temporary file");
                return 0;
            }

            try
            {
                using (var hive = OffregHive.Open(copyName))
                {
                    var root = hive.Root.OpenSubKey("Root");
                    foreach (var subkeyName in root.GetSubKeyNames())
                    {
                        if (!subkeyName.StartsWith("InventoryApplication"))
                            continue;

                        var subkey = root.OpenSubKey(subkeyName);
                        foreach (var subname2 in subkey.GetSubKeyNames()) // drop all subkeys
                        {
#if !DEBUG
                            subkey.DeleteSubKeyTree(subname2);
#endif
                            Log.Information("Eliminated Amcache hive subkey {key} -> {keyname}", subkey.FullName, subname2);
                        }
                    }

#if DEBUG
                    var copyName2 = Path.GetRandomFileName();
                    hive.SaveHive(copyName2, 6u, 1u); // Windows 7 ...?
                    Log.Information("The hive re-saved to: {path}", copyName2);
#else
                    hive.SaveHive(path, 6u, 1u); // Windows 7 ...?
#endif
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error writing Amcache.hve hive");
            }
            finally
            {
                File.Delete(copyName);
            }

            return 0;
        }
    }
}

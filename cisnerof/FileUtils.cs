using Serilog;
using System;
using System.IO;
using System.Linq;

namespace cisnerof
{
    internal static class FileUtils
    {
        public static int EliminateSingleFile(string path)
        {
            var item = new FileInfo(path);
            if (item.Exists)
            {
#if !DEBUG
                item.Delete();
#endif
                Log.Debug("Eliminated single item {path}", item);
                return 1;
            }

            return 0;
        }

        private static DirectoryInfo CheckValidDirectory(string path)
        {
            if (!Path.IsPathRooted(path))
            {
                Log.Error("The path {path} is not a valid absolute path.");
                return null;
            }

            var dirInfo = new DirectoryInfo(path);
            if (!dirInfo.Exists)
            {
                Log.Warning("Directory {dir} does not exists.", path);
                return null;
            }

            return dirInfo;
        }

        public static int EliminateFolderSubitems(string path, string filter = "*", bool recursive = true)
        {
            var dir = CheckValidDirectory(path);
            if (dir == null)
                return 0;

            var count = 0;
            try
            {
                var files = dir.EnumerateFileSystemInfos(filter, recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
                foreach (var item in files)
                {
                    try
                    {
#if !DEBUG
                        item.Delete();
#endif
                        Log.Debug("Eliminated folder sub-item {path}", item);
                    }
                    catch (Exception ex)
                    {
                        Log.Warning(ex, "Error eliminating item {path}", item);
                    }
                }

                count = files.Count();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error eliminating the sub-items of path {path}.", path);
            }

            return count;
        }

        public static int EliminateFolderSubitems(string path, Func<FileSystemInfo, bool> filter, bool recursive = true)
        {
            var dir = CheckValidDirectory(path);
            if (dir == null)
                return 0;

            var count = 0;
            try
            {
                var files = dir.EnumerateFileSystemInfos("*", recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
                foreach (var item in files)
                {
                    try
                    {
                        if (!filter(item))
                            continue;

#if !DEBUG
                        item.Delete();
#endif
                        Log.Debug("Eliminated folder sub-item {path}", item);
                    }
                    catch (Exception ex)
                    {
                        Log.Warning(ex, "Error eliminating item {path}", item);
                    }
                }

                count = files.Count();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error eliminating the sub-items of path {path}.", path);
            }

            return count;
        }
    }
}

using AntiForensicLib;
using System;
using System.IO;
using System.Linq;

namespace AntiForensicLib
{
    public static class FileUtils
    {
        public static int EliminateSingleFile(string path)
        {
            var item = new FileInfo(path);
            if (item.Exists)
            {
#if !DEBUG
                item.Delete();
#endif
                Facade.Logger.Debug(string.Format("Eliminated single item: {0}", item));
                return 1;
            }

            return 0;
        }

        private static DirectoryInfo CheckValidDirectory(string path)
        {
            if (!Path.IsPathRooted(path))
            {
                Facade.Logger.Error(string.Format("Not a valid absolute path: {0}", path));
                return null;
            }

            var dirInfo = new DirectoryInfo(path);
            if (!dirInfo.Exists)
            {
                Facade.Logger.Warning(string.Format("Directory does not exists: {0}", path));
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
                        Facade.Logger.Debug(string.Format("Eliminated folder sub-item: {0}", item));
                    }
                    catch (Exception ex)
                    {
                        Facade.Logger.Error(ex, string.Format("Error eliminating item: {0}", item));
                    }
                }

                count = files.Count();
            }
            catch (Exception ex)
            {
                Facade.Logger.Error(ex, string.Format("Error eliminating the sub-items of path: {0}", path));
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
                        Facade.Logger.Debug(string.Format("Eliminated folder sub-item: {0}", item));
                    }
                    catch (Exception ex)
                    {
                        Facade.Logger.Error(ex, string.Format("Error eliminating item: {0}", item));
                    }
                }

                count = files.Count();
            }
            catch (Exception ex)
            {
                Facade.Logger.Error(ex, string.Format("Error eliminating the sub-items of path: {0}", path));
            }

            return count;
        }
    }
}

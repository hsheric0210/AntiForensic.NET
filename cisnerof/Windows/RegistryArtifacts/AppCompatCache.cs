using Serilog;
using System;
using System.Runtime.InteropServices;

namespace cisnerof.Windows.RegistryArtifacts
{
    /// <summary>
    /// https://www.sevenforums.com/general-discussion/384201-appcompatcache-registry-windows-7-a.html
    /// </summary>
    internal class AppCompatCache : ICleaner
    {
        public string Name => "Application Compatibility Cache (AppCompatCache) & ShimCache";

        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool BaseFlushAppcompatCache();

        [DllImport("apphelp.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool ShimFlushCache(IntPtr hwnd, IntPtr hInstance, IntPtr lpszCmdLine, int nCmdShow);

        public int RunCleaner()
        {
            try
            {
#if DEBUG
                var result = true;
#else
                var result = BaseFlushAppcompatCache() && ShimFlushCache(IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, 0);
#endif
                if (!result)
                    Log.Warning("Failed to flush AppCompatCache & ShimCache. You must execute the program as administrator right to completely flush the cache.");

                return result ? 1 : 0;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error flushing AppCompatCache");
            }

            return 0;
        }
    }
}

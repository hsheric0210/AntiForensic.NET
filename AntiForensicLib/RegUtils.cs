using AntiForensicLib;
using Microsoft.Win32;
using System;

namespace AntiForensicLib
{
    // 참고: HKCU = HKU\\S-1-5-9-... (현재 유저 SID)
    // https://www.tenforums.com/general-support/75526-hkcu-hku-registry-records.html
    internal static class RegUtils
    {
        public static int EliminateKeySubentries(RegistryKey key, Func<string, bool> filter = null)
        {
            var count = 0;
            if (key == null)
                return count;

            try
            {
                var names = key.GetValueNames();
                foreach (var value in names)
                {
                    if (filter != null && !filter(value))
                        continue;

                    try
                    {
#if !DEBUG
                        key.DeleteValue(value);
#endif
                        Facade.Logger.Debug(string.Format("Eliminated registry value: {0} -> {1}", key.Name, value));
                    }
                    catch (Exception ex)
                    {
                        Facade.Logger.Error(ex, string.Format("Error eliminating registry value: {0} -> {1}", key.Name, value));
                    }
                }

                count = names.Length;
            }
            catch (Exception ex)
            {
                Facade.Logger.Error(ex, string.Format("Error eliminating sub-values of registry key: {0}", key.Name));
            }

            return count;
        }

        public static int EliminateKeySubentriesRecursive(RegistryKey key)
        {
            var count = 0;
            if (key == null)
                return count;

            count += EliminateKeySubentries(key);

            try
            {
                var names = key.GetSubKeyNames();
                foreach (var subkey in names)
                {
                    count += EliminateKeySubentries(key.OpenSubKey(subkey));
#if !DEBUG
                    key.DeleteSubKeyTree(subkey);
#endif
                }

                count = names.Length;
            }
            catch (Exception ex)
            {
                Facade.Logger.Error(ex, string.Format("Error eliminating sub-keys of registry key: {0}", key.Name));
            }

            return count;
        }
    }
}
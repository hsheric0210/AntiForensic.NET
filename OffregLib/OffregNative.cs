using OffregLib.Properties;
using StealthModule;
using System;
using System.Runtime.InteropServices;
using System.Text;
using FILETIME = System.Runtime.InteropServices.ComTypes.FILETIME;

namespace OffregLib
{
    public enum RegValueType : uint
    {
        REG_NONE = 0,
        REG_SZ = 1,
        REG_EXPAND_SZ = 2,
        REG_BINARY = 3,
        REG_DWORD = 4,
        REG_DWORD_LITTLE_ENDIAN = 4,
        REG_DWORD_BIG_ENDIAN = 5,
        REG_LINK = 6,
        REG_MULTI_SZ = 7,
        REG_RESOURCE_LIST = 8,
        REG_FULL_RESOURCE_DESCRIPTOR = 9,
        REG_RESOURCE_REQUIREMENTS_LIST = 10,
        REG_QWORD = 11,
        REG_QWORD_LITTLE_ENDIAN = 11
    }

    public enum RegPredefinedKeys
    {
        HKEY_CLASSES_ROOT = unchecked((int)0x80000000),
        HKEY_CURRENT_USER = unchecked((int)0x80000001),
        HKEY_LOCAL_MACHINE = unchecked((int)0x80000002),
        HKEY_USERS = unchecked((int)0x80000003),
        HKEY_PERFORMANCE_DATA = unchecked((int)0x80000004),
        HKEY_CURRENT_CONFIG = unchecked((int)0x80000005),
        HKEY_DYN_DATA = unchecked((int)0x80000006),
        HKEY_CURRENT_USER_LOCAL_SETTINGS = unchecked((int)0x80000007)
    }

    public enum KeyDisposition : long
    {
        REG_CREATED_NEW_KEY = 0x00000001,
        REG_OPENED_EXISTING_KEY = 0x00000002
    }

    public enum KeySecurity
    {
        KEY_QUERY_VALUE = 0x0001,
        KEY_SET_VALUE = 0x0002,
        KEY_ENUMERATE_SUB_KEYS = 0x0008,
        KEY_NOTIFY = 0x0010,
        DELETE = 0x10000,
        STANDARD_RIGHTS_READ = 0x20000,
        KEY_READ = 0x20019,
        KEY_WRITE = 0x20006,
        KEY_ALL_ACCESS = 0xF003F,
        MAXIMUM_ALLOWED = 0x2000000
    }

    [Flags]
    public enum RegOption : uint
    {
        REG_OPTION_RESERVED = 0x00000000,
        REG_OPTION_NON_VOLATILE = 0x00000000,
        REG_OPTION_VOLATILE = 0x00000001,
        REG_OPTION_CREATE_LINK = 0x00000002,
        REG_OPTION_BACKUP_RESTORE = 0x00000004,
        REG_OPTION_OPEN_LINK = 0x00000008
    }

    public enum SECURITY_INFORMATION : uint
    {
        OWNER_SECURITY_INFORMATION = 0x00000001,
        GROUP_SECURITY_INFORMATION = 0x00000002,
        DACL_SECURITY_INFORMATION = 0x00000004,
        SACL_SECURITY_INFORMATION = 0x00000008,
        LABEL_SECURITY_INFORMATION = 0x00000010,
        PROTECTED_DACL_SECURITY_INFORMATION = 0x80000000,
        PROTECTED_SACL_SECURITY_INFORMATION = 0x40000000,
        UNPROTECTED_DACL_SECURITY_INFORMATION = 0x20000000,
        UNPROTECTED_SACL_SECURITY_INFORMATION = 0x10000000,
    }

    /// <summary>
    ///     All the functions can be read about here:
    ///     http://msdn.microsoft.com/en-us/library/ee210756(v=vs.85).aspx
    /// </summary>
    public static class OffregNative
    {
        private static MemoryModule offreg = null; // Prevent from being GC'd

        private static void InitNative()
        {
            if (offreg != null)
                return;

            var dll = IntPtr.Size == 8 ? Resources.offreg_x64 : Resources.offreg_x86;
            offreg = new MemoryModule(dll);

            createHive = offreg.GetExport<CreateHiveDelegate>("ORCreateHive");
            openHive = offreg.GetExport<OpenHiveDelegate>("OROpenHive");
            closeHive = offreg.GetExport<CloseHiveDelegate>("ORCloseHive");
            saveHive = offreg.GetExport<SaveHiveDelegate>("ORSaveHive");
            closeKey = offreg.GetExport<CloseKeyDelegate>("ORCloseKey");
            createKey = offreg.GetExport<CreateKeyDelegate>("ORCreateKey");
            deleteKey = offreg.GetExport<DeleteKeyDelegate>("ORDeleteKey");
            deleteValue = offreg.GetExport<DeleteValueDelegate>("ORDeleteValue");
            enumKeyRef = offreg.GetExport<EnumKeyDelegate_ref>("OREnumKey");
            enumKeyPtr = offreg.GetExport<EnumKeyDelegate_ptr>("OREnumKey");
            enumValueRef = offreg.GetExport<EnumValueDelegate_ref>("OREnumValue");
            enumValuePtr = offreg.GetExport<EnumValueDelegate_ptr>("OREnumValue");
            getKeySecurity = offreg.GetExport<GetKeySecurityDelegate>("ORGetKeySecurity");
            getValueRef = offreg.GetExport<GetValueDelegate_ref>("ORGetValue");
            getValuePtr = offreg.GetExport<GetValueDelegate_ptr>("ORGetValue");
            openKey = offreg.GetExport<OpenKeyDelegate>("OROpenKey");
            queryInfoKey = offreg.GetExport<QueryInfoKeyDelegate>("ORQueryInfoKey");
            setValue = offreg.GetExport<SetValueDelegate>("ORSetValue");
            setKeySecurity = offreg.GetExport<SetKeySecurityDelegate>("ORSetKeySecurity");
        }

        [UnmanagedFunctionPointer(CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        private delegate Win32Result CreateHiveDelegate(out IntPtr rootKeyHandle);

        [UnmanagedFunctionPointer(CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        private delegate Win32Result OpenHiveDelegate(string path, out IntPtr rootKeyHandle);

        [UnmanagedFunctionPointer(CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        private delegate Win32Result CloseHiveDelegate(IntPtr rootKeyHandle);

        [UnmanagedFunctionPointer(CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        private delegate Win32Result SaveHiveDelegate(IntPtr rootKeyHandle, string path, uint dwOsMajorVersion, uint dwOsMinorVersion);

        [UnmanagedFunctionPointer(CallingConvention.Winapi)]
        private delegate Win32Result CloseKeyDelegate(IntPtr hKey);

        [UnmanagedFunctionPointer(CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        private delegate Win32Result CreateKeyDelegate(IntPtr hKey, string lpSubKey, string lpClass, RegOption dwOptions, /*ref SECURITY_DESCRIPTOR*/ IntPtr lpSecurityDescriptor, /*ref IntPtr*/ out IntPtr phkResult, out KeyDisposition lpdwDisposition);

        [UnmanagedFunctionPointer(CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        private delegate Win32Result DeleteKeyDelegate(IntPtr hKey, string lpSubKey);

        [UnmanagedFunctionPointer(CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        private delegate Win32Result DeleteValueDelegate(IntPtr hKey, string lpValueName);

        [UnmanagedFunctionPointer(CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        private delegate Win32Result EnumKeyDelegate_ref(IntPtr hKey, uint dwIndex, StringBuilder lpName, ref uint lpcchName, StringBuilder lpClass, ref uint lpcchClass, ref FILETIME lpftLastWriteTime);

        [UnmanagedFunctionPointer(CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        private delegate Win32Result EnumKeyDelegate_ptr(IntPtr hKey, uint dwIndex, StringBuilder lpName, ref uint lpcchName, StringBuilder lpClass, IntPtr lpcchClass, IntPtr lpftLastWriteTime);

        [UnmanagedFunctionPointer(CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        private delegate Win32Result EnumValueDelegate_ref(IntPtr hKey, uint dwIndex, StringBuilder lpValueName, ref uint lpcchValueName, out RegValueType lpType, IntPtr lpData, ref uint lpcbData);

        [UnmanagedFunctionPointer(CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        private delegate Win32Result EnumValueDelegate_ptr(IntPtr hKey, uint dwIndex, StringBuilder lpValueName, ref uint lpcchValueName, IntPtr lpType, IntPtr lpData, IntPtr lpcbData);

        [UnmanagedFunctionPointer(CallingConvention.Winapi)]
        private delegate Win32Result GetKeySecurityDelegate(IntPtr hKey, SECURITY_INFORMATION securityInformation, IntPtr pSecurityDescriptor, ref uint lpcbSecurityDescriptor);

        [UnmanagedFunctionPointer(CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        private delegate Win32Result GetValueDelegate_ref(IntPtr hKey, string lpSubKey, string lpValue, out RegValueType pdwType, IntPtr pvData, ref uint pcbData);

        [UnmanagedFunctionPointer(CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        private delegate Win32Result GetValueDelegate_ptr(IntPtr hKey, string lpSubKey, string lpValue, out RegValueType pdwType, IntPtr pvData, IntPtr pcbData);

        [UnmanagedFunctionPointer(CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        private delegate Win32Result OpenKeyDelegate(IntPtr hKey, string lpSubKey, out IntPtr phkResult);

        [UnmanagedFunctionPointer(CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        private delegate Win32Result QueryInfoKeyDelegate(IntPtr hKey, StringBuilder lpClass, ref uint lpcchClass, ref uint lpcSubKeys, ref uint lpcbMaxSubKeyLen, ref uint lpcbMaxClassLen, ref uint lpcValues, ref uint lpcbMaxValueNameLen, ref uint lpcbMaxValueLen, ref uint lpcbSecurityDescriptor, ref FILETIME lpftLastWriteTime);

        [UnmanagedFunctionPointer(CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        private delegate Win32Result SetValueDelegate(IntPtr hKey, string lpValueName, RegValueType dwType, IntPtr lpData, uint cbData);

        [UnmanagedFunctionPointer(CallingConvention.Winapi)]
        private delegate Win32Result SetKeySecurityDelegate(IntPtr hKey, SECURITY_INFORMATION securityInformation, /*ref IntPtr*/ IntPtr pSecurityDescriptor);

        private static CreateHiveDelegate createHive;
        private static OpenHiveDelegate openHive;
        private static CloseHiveDelegate closeHive;
        private static SaveHiveDelegate saveHive;
        private static CloseKeyDelegate closeKey;
        private static CreateKeyDelegate createKey;
        private static DeleteKeyDelegate deleteKey;
        private static DeleteValueDelegate deleteValue;
        private static EnumKeyDelegate_ref enumKeyRef;
        private static EnumKeyDelegate_ptr enumKeyPtr;
        private static EnumValueDelegate_ref enumValueRef;
        private static EnumValueDelegate_ptr enumValuePtr;
        private static GetKeySecurityDelegate getKeySecurity;
        private static GetValueDelegate_ref getValueRef;
        private static GetValueDelegate_ptr getValuePtr;
        private static OpenKeyDelegate openKey;
        private static QueryInfoKeyDelegate queryInfoKey;
        private static SetValueDelegate setValue;
        private static SetKeySecurityDelegate setKeySecurity;

        /// <summary>
        ///     Create a new Registry Hive
        ///     See http://msdn.microsoft.com/en-us/library/2d6dt3kf.aspx
        /// </summary>
        /// <param name="rootKeyHandle">The handle to the new hive.</param>
        /// <returns>
        ///     <see cref="Win32Result" /> of the result. Win32Result.ERROR_SUCCESS indicates success.
        /// </returns>
        public static Win32Result CreateHive(out IntPtr rootKeyHandle)
        {
            if (createHive == null)
                InitNative();

            return createHive(out rootKeyHandle);
        }

        /// <summary>
        ///     Open an existing Registry Hive
        ///     See http://msdn.microsoft.com/en-us/library/ee210770(v=vs.85).aspx
        /// </summary>
        /// <param name="path">The path to the hive file.</param>
        /// <param name="rootKeyHandle">The handle to an open hive.</param>
        /// <returns>
        ///     <see cref="Win32Result" /> of the result. Win32Result.ERROR_SUCCESS indicates success.
        /// </returns>
        public static Win32Result OpenHive(string path, out IntPtr rootKeyHandle)
        {
            if (openHive == null)
                InitNative();

            return openHive(path, out rootKeyHandle);
        }

        /// <summary>
        ///     Close an open hive, freeing ressources used by it.
        ///     See http://msdn.microsoft.com/en-us/library/ee210758(v=vs.85).aspx
        /// </summary>
        /// <seealso cref="SaveHive" />
        /// <remarks>
        ///     This does not save a hive to disk, to preserve changes, see <see cref="SaveHive" />.
        /// </remarks>
        /// <param name="rootKeyHandle">The handle to an open hive.</param>
        /// <returns>
        ///     <see cref="Win32Result" /> of the result. Win32Result.ERROR_SUCCESS indicates success.
        /// </returns>
        public static Win32Result CloseHive(IntPtr rootKeyHandle)
        {
            if (closeHive == null)
                InitNative();

            return closeHive(rootKeyHandle);
        }

        /// <summary>
        ///     Save an open hive to disk.
        ///     This saves the hive with a specific compatibility option. See the link below for more details.
        ///     See http://msdn.microsoft.com/en-us/library/ee210773(v=vs.85).aspx
        /// </summary>
        /// <param name="rootKeyHandle">The handle to the open hive</param>
        /// <param name="path">The path to a non-existent file in which to save the hive</param>
        /// <param name="dwOsMajorVersion">The major os version to save the hive for. See summary.</param>
        /// <param name="dwOsMinorVersion">The minor os version to save the hive for. See summary.</param>
        /// <returns>
        ///     <see cref="Win32Result" /> of the result. Win32Result.ERROR_SUCCESS indicates success.
        /// </returns>
        public static Win32Result SaveHive(IntPtr rootKeyHandle, string path, uint dwOsMajorVersion, uint dwOsMinorVersion)
        {
            if (saveHive == null)
                InitNative();

            return saveHive(rootKeyHandle, path, dwOsMajorVersion, dwOsMinorVersion);
        }

        /// <summary>
        ///     Close an open key.
        ///     See http://msdn.microsoft.com/en-us/library/ee210759(v=vs.85).aspx
        /// </summary>
        /// <param name="hKey">The handle to an open key.</param>
        /// <returns>
        ///     <see cref="Win32Result" /> of the result. Win32Result.ERROR_SUCCESS indicates success.
        /// </returns>
        public static Win32Result CloseKey(IntPtr hKey)
        {
            if (closeKey == null)
                InitNative();

            return closeKey(hKey);
        }

        /// <summary>
        ///     Create a new subkey (or open an existing one) under another key.
        ///     See http://msdn.microsoft.com/en-us/library/ee210761(v=vs.85).aspx
        /// </summary>
        /// <param name="hKey">Handle to an open key.</param>
        /// <param name="lpSubKey">Name of the new subkey.</param>
        /// <param name="lpClass">Name of the type of the new subkey.</param>
        /// <param name="dwOptions">Options for the creation.</param>
        /// <param name="lpSecurityDescriptor">Security descripter, may be NULL.</param>
        /// <param name="phkResult">The handle to the newly created key.</param>
        /// <param name="lpdwDisposition">The reuslting disposition.</param>
        /// <returns>
        ///     <see cref="Win32Result" /> of the result. Win32Result.ERROR_SUCCESS indicates success.
        /// </returns>
        public static Win32Result CreateKey(IntPtr hKey, string lpSubKey, string lpClass, RegOption dwOptions, /*ref SECURITY_DESCRIPTOR*/ IntPtr lpSecurityDescriptor, /*ref IntPtr*/ out IntPtr phkResult, out KeyDisposition lpdwDisposition)
        {
            if (createKey == null)
                InitNative();

            return createKey(hKey, lpSubKey, lpClass, dwOptions, lpSecurityDescriptor, out phkResult, out lpdwDisposition);
        }

        /// <summary>
        ///     Delete a subkey.
        ///     See http://msdn.microsoft.com/en-us/library/ee210762(v=vs.85).aspx
        /// </summary>
        /// <param name="hKey">Handle to an open parent key.</param>
        /// <param name="lpSubKey">Name of the subkey, in the parent, to delete. Null indicates that the parent should be deleted.</param>
        /// <returns>
        ///     <see cref="Win32Result" /> of the result. Win32Result.ERROR_SUCCESS indicates success.
        /// </returns>
        public static Win32Result DeleteKey(IntPtr hKey, string lpSubKey)
        {
            if (deleteKey == null)
                InitNative();

            return deleteKey(hKey, lpSubKey);
        }

        /// <summary>
        ///     Delete a value under a key.
        ///     See http://msdn.microsoft.com/en-us/library/ee210763(v=vs.85).aspx
        /// </summary>
        /// <param name="hKey">Handle to an open parent key.</param>
        /// <param name="lpValueName">Name of the value to delete.</param>
        /// <returns>
        ///     <see cref="Win32Result" /> of the result. Win32Result.ERROR_SUCCESS indicates success.
        /// </returns>
        public static Win32Result DeleteValue(IntPtr hKey, string lpValueName)
        {
            if (deleteValue == null)
                InitNative();

            return deleteValue(hKey, lpValueName);
        }

        /// <summary>
        ///     Enumerate keys under a parent.
        ///     See http://msdn.microsoft.com/en-us/library/ee210764(v=vs.85).aspx
        /// </summary>
        /// <param name="hKey">Handle to an open parent key.</param>
        /// <param name="dwIndex">Index of the child to retrieve.</param>
        /// <param name="lpName">Buffer for the childs name.</param>
        /// <param name="lpcchName">Size of the childs name buffer.</param>
        /// <param name="lpClass">Buffer for the childs class.</param>
        /// <param name="lpcchClass">Size of the childs class buffer.</param>
        /// <param name="lpftLastWriteTime">FileTime structure indicating last write time.</param>
        /// <returns>
        ///     <see cref="Win32Result" /> of the result. Win32Result.ERROR_SUCCESS indicates success, Win32Result.ERROR_NO_MORE_ITEMS indicates that no more childs exist.
        /// </returns>
        public static Win32Result EnumKey(IntPtr hKey, uint dwIndex, StringBuilder lpName, ref uint lpcchName, StringBuilder lpClass, ref uint lpcchClass, ref FILETIME lpftLastWriteTime)
        {
            if (enumKeyRef == null)
                InitNative();

            return enumKeyRef(hKey, dwIndex, lpName, ref lpcchName, lpClass, ref lpcchClass, ref lpftLastWriteTime);
        }

        /// <summary>
        ///     Enumerate keys under a parent.
        ///     See http://msdn.microsoft.com/en-us/library/ee210764(v=vs.85).aspx
        /// </summary>
        /// <param name="hKey">Handle to an open parent key.</param>
        /// <param name="dwIndex">Index of the child to retrieve.</param>
        /// <param name="lpName">Buffer for the childs name.</param>
        /// <param name="lpcchName">Size of the childs name buffer.</param>
        /// <param name="lpClass">Unused - set to Intpr.Zero.</param>
        /// <param name="lpcchClass">Unused - set to Intpr.Zero.</param>
        /// <param name="lpftLastWriteTime">Unused - set to Intpr.Zero.</param>
        /// <returns>
        ///     <see cref="Win32Result" /> of the result. Win32Result.ERROR_SUCCESS indicates success, Win32Result.ERROR_NO_MORE_ITEMS indicates that no more childs exist.
        /// </returns>
        public static Win32Result EnumKey(IntPtr hKey, uint dwIndex, StringBuilder lpName, ref uint lpcchName, StringBuilder lpClass, IntPtr lpcchClass, IntPtr lpftLastWriteTime)
        {
            if (enumKeyPtr == null)
                InitNative();

            return enumKeyPtr(hKey, dwIndex, lpName, ref lpcchName, lpClass, lpcchClass, lpftLastWriteTime);
        }

        /// <summary>
        ///     Enumerate a keys values.
        ///     See http://msdn.microsoft.com/en-us/library/ee210765(v=vs.85).aspx
        /// </summary>
        /// <param name="hKey">Handle to an open parent key.</param>
        /// <param name="dwIndex">Index of the child to retrieve.</param>
        /// <param name="lpValueName">Buffer for the childs name.</param>
        /// <param name="lpcchValueName">Size of the childs name buffer.</param>
        /// <param name="lpType">Value type.</param>
        /// <param name="lpData">Pointer to data buffer.</param>
        /// <param name="lpcbData">Size of data buffer.</param>
        /// <returns>
        ///     <see cref="Win32Result" /> of the result. Win32Result.ERROR_SUCCESS indicates success.
        /// </returns>
        public static Win32Result EnumValue(IntPtr hKey, uint dwIndex, StringBuilder lpValueName, ref uint lpcchValueName, out RegValueType lpType, IntPtr lpData, ref uint lpcbData)
        {
            if (enumValueRef == null)
                InitNative();

            return enumValueRef(hKey, dwIndex, lpValueName, ref lpcchValueName, out lpType, lpData, ref lpcbData);
        }

        /// <summary>
        ///     Enumerate a keys values.
        ///     See http://msdn.microsoft.com/en-us/library/ee210765(v=vs.85).aspx
        /// </summary>
        /// <param name="hKey">Handle to an open parent key.</param>
        /// <param name="dwIndex">Index of the child to retrieve.</param>
        /// <param name="lpValueName">Buffer for the childs name.</param>
        /// <param name="lpcchValueName">Size of the childs name buffer.</param>
        /// <param name="lpType">Unused - set to Intpr.Zero.</param>
        /// <param name="lpData">Unused - set to Intpr.Zero.</param>
        /// <param name="lpcbData">Unused - set to Intpr.Zero.</param>
        /// <returns>
        ///     <see cref="Win32Result" /> of the result. Win32Result.ERROR_SUCCESS indicates success.
        /// </returns>
        public static Win32Result EnumValue(IntPtr hKey, uint dwIndex, StringBuilder lpValueName, ref uint lpcchValueName, IntPtr lpType, IntPtr lpData, IntPtr lpcbData)
        {
            if (enumValuePtr == null)
                InitNative();

            return enumValuePtr(hKey, dwIndex, lpValueName, ref lpcchValueName, lpType, lpData, lpcbData);
        }

        /// <summary>
        ///     Gets a keys security descriptor.
        ///     See http://msdn.microsoft.com/en-us/library/ee210766(v=vs.85).aspx
        /// </summary>
        /// <param name="hKey">Handle to an open key.</param>
        /// <param name="securityInformation">The type of security information to request.</param>
        /// <param name="pSecurityDescriptor">Pointer to data buffer.</param>
        /// <param name="lpcbSecurityDescriptor">Size of the data buffer.</param>
        /// <returns>
        ///     <see cref="Win32Result" /> of the result. Win32Result.ERROR_SUCCESS indicates success.
        /// </returns>
        public static Win32Result GetKeySecurity(IntPtr hKey, SECURITY_INFORMATION securityInformation, IntPtr pSecurityDescriptor, ref uint lpcbSecurityDescriptor)
        {
            if (getKeySecurity == null)
                InitNative();

            return getKeySecurity(hKey, securityInformation, pSecurityDescriptor, ref lpcbSecurityDescriptor);
        }

        /// <summary>
        ///     Gets a value under a key.
        ///     See http://msdn.microsoft.com/en-us/library/ee210767(v=vs.85).aspx
        /// </summary>
        /// <param name="hKey">Handle to an open key.</param>
        /// <param name="lpSubKey">The name of the subkey under the parent, from which to retrieve the value. May be null.</param>
        /// <param name="lpValue">Name of the value to retrieve.</param>
        /// <param name="pdwType">The type of the value.</param>
        /// <param name="pvData">Pointer to a data buffer.</param>
        /// <param name="pcbData">Size of the data buffer.</param>
        /// <returns>
        ///     <see cref="Win32Result" /> of the result. Win32Result.ERROR_SUCCESS indicates success.
        /// </returns>
        public static Win32Result GetValue(IntPtr hKey, string lpSubKey, string lpValue, out RegValueType pdwType, IntPtr pvData, ref uint pcbData)
        {
            if (getValueRef == null)
                InitNative();

            return getValueRef(hKey, lpSubKey, lpValue, out pdwType, pvData, ref pcbData);
        }

        /// <summary>
        ///     Gets a value under a key.
        ///     See http://msdn.microsoft.com/en-us/library/ee210767(v=vs.85).aspx
        /// </summary>
        /// <param name="hKey">Handle to an open key.</param>
        /// <param name="lpSubKey">The name of the subkey under the parent, from which to retrieve the value. May be null.</param>
        /// <param name="lpValue">Name of the value to retrieve.</param>
        /// <param name="pdwType">The type of the value.</param>
        /// <param name="pvData">Unused - set to Intpr.Zero.</param>
        /// <param name="pcbData">Unused - set to Intpr.Zero.</param>
        /// <returns>
        ///     <see cref="Win32Result" /> of the result. Win32Result.ERROR_SUCCESS indicates success.
        /// </returns>
        public static Win32Result GetValue(IntPtr hKey, string lpSubKey, string lpValue, out RegValueType pdwType, IntPtr pvData, IntPtr pcbData)
        {
            if (getValuePtr == null)
                InitNative();

            return getValuePtr(hKey, lpSubKey, lpValue, out pdwType, pvData, pcbData);
        }

        /// <summary>
        ///     Open a subkey.
        ///     See http://msdn.microsoft.com/en-us/library/ee210771(v=vs.85).aspx
        /// </summary>
        /// <param name="hKey">Handle to an open parent key.</param>
        /// <param name="lpSubKey">Name of the subkey to open.</param>
        /// <param name="phkResult">Handle to the opened subkey.</param>
        /// <returns>
        ///     <see cref="Win32Result" /> of the result. Win32Result.ERROR_SUCCESS indicates success.
        /// </returns>
        public static Win32Result OpenKey(IntPtr hKey, string lpSubKey, out IntPtr phkResult)
        {
            if (openKey == null)
                InitNative();

            return openKey(hKey, lpSubKey, out phkResult);
        }

        /// <summary>
        ///     Query details about a key.
        ///     See http://msdn.microsoft.com/en-us/library/ee210772(v=vs.85).aspx
        /// </summary>
        /// <param name="hKey">Handle to an open key.</param>
        /// <param name="lpClass">The keys class.</param>
        /// <param name="lpcchClass">The size of the class string in chars.</param>
        /// <param name="lpcSubKeys">The number of subkeys.</param>
        /// <param name="lpcbMaxSubKeyLen">The largest name of a subkey.</param>
        /// <param name="lpcbMaxClassLen">The largest subkey class size.</param>
        /// <param name="lpcValues">The number of values.</param>
        /// <param name="lpcbMaxValueNameLen">The largest name of a value.</param>
        /// <param name="lpcbMaxValueLen">The largest values size.</param>
        /// <param name="lpcbSecurityDescriptor">The size of the security descriptor for this key.</param>
        /// <param name="lpftLastWriteTime">The last time the key was written to.</param>
        /// <returns>
        ///     <see cref="Win32Result" /> of the result. Win32Result.ERROR_SUCCESS indicates success.
        /// </returns>
        public static Win32Result QueryInfoKey(IntPtr hKey, StringBuilder lpClass, ref uint lpcchClass, ref uint lpcSubKeys, ref uint lpcbMaxSubKeyLen, ref uint lpcbMaxClassLen, ref uint lpcValues, ref uint lpcbMaxValueNameLen, ref uint lpcbMaxValueLen, ref uint lpcbSecurityDescriptor, ref FILETIME lpftLastWriteTime)
        {
            if (queryInfoKey == null)
                InitNative();

            return queryInfoKey(hKey, lpClass, ref lpcchClass, ref lpcSubKeys, ref lpcbMaxSubKeyLen, ref lpcbMaxClassLen, ref lpcValues, ref lpcbMaxValueNameLen, ref lpcbMaxValueLen, ref lpcbSecurityDescriptor, ref lpftLastWriteTime);
        }

        /// <summary>
        ///     Sets a value.
        ///     See http://msdn.microsoft.com/en-us/library/ee210775(v=vs.85).aspx
        /// </summary>
        /// <param name="hKey">Handle to an open key.</param>
        /// <param name="lpValueName">The name of the value.</param>
        /// <param name="dwType">The type of the value.</param>
        /// <param name="lpData">The data buffer to save in the value.</param>
        /// <param name="cbData">The size of the data buffer.</param>
        /// <returns>
        ///     <see cref="Win32Result" /> of the result. Win32Result.ERROR_SUCCESS indicates success.
        /// </returns>
        public static Win32Result SetValue(IntPtr hKey, string lpValueName, RegValueType dwType, IntPtr lpData, uint cbData)
        {
            if (setValue == null)
                InitNative();

            return setValue(hKey, lpValueName, dwType, lpData, cbData);
        }

        /// <summary>
        ///     Sets a keys security descriptor.
        ///     See http://msdn.microsoft.com/en-us/library/ee210774(v=vs.85).aspx
        /// </summary>
        /// <param name="hKey">Handle to an open key.</param>
        /// <param name="securityInformation">The type of security information to set.</param>
        /// <param name="pSecurityDescriptor">Pointer to data buffer.</param>
        /// <returns>
        ///     <see cref="Win32Result" /> of the result. Win32Result.ERROR_SUCCESS indicates success.
        /// </returns>
        public static Win32Result SetKeySecurity(IntPtr hKey, SECURITY_INFORMATION securityInformation, /*ref IntPtr*/ IntPtr pSecurityDescriptor)
        {
            if (setKeySecurity == null)
                InitNative();

            return setKeySecurity(hKey, securityInformation, pSecurityDescriptor);
        }
    }
}
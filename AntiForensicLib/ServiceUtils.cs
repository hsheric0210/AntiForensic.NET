using AntiForensicLib;
using System;
using System.Runtime.InteropServices;

namespace AntiForensicLib
{
    internal static class ServiceUtils
    {

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern IntPtr OpenSCManagerA(string lpMachineName, string lpDatabaseName, uint dwDesiredAccess);

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern IntPtr OpenServiceA(IntPtr hSCManager, string lpServiceName, uint dwDesiredAccess);

        [DllImport("advapi32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool ControlService(IntPtr hService, uint dwControl, IntPtr lpServiceStatus);

        [DllImport("advapi32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool StartServiceA(IntPtr hService, uint dwNumServiceArgs, IntPtr lpServiceArgVectors);

        [DllImport("advapi32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool CloseServiceHandle(IntPtr hSCObject);

        private const uint SC_MANAGER_CONNECT = 0x0001;

        private const uint SERVICE_START = 0x0010;
        private const uint SERVICE_STOP = 0x0020;

        private const uint SERVICE_CONTROL_STOP = 0x00000001;

        public static void StartService(string serviceName)
        {
            var svc = IntPtr.Zero;
            var scm = OpenSCManagerA(null, null, SC_MANAGER_CONNECT);
            if (scm == IntPtr.Zero)
            {
                Facade.Logger.Error(string.Format("Failed to open Service Controller Manager. Win32 error {0}.", Marshal.GetLastWin32Error()));
                goto cleanup;
            }

            svc = OpenServiceA(scm, serviceName, SERVICE_START);
            if (scm == IntPtr.Zero)
            {
                Facade.Logger.Error(string.Format("Failed to open service {0}. Win32 error {1}.", serviceName, Marshal.GetLastWin32Error()));
                goto cleanup;
            }

            if (!StartServiceA(svc, 0, IntPtr.Zero))
            {
                Facade.Logger.Error(string.Format("Failed to start service {0}. Win32 error {1}.", serviceName, Marshal.GetLastWin32Error()));
            }

cleanup:
            if (svc != IntPtr.Zero)
                CloseServiceHandle(svc);
            if (scm != IntPtr.Zero)
                CloseServiceHandle(scm);
        }

        public static void StopService(string serviceName)
        {
            var svc = IntPtr.Zero;
            var scm = OpenSCManagerA(null, null, SC_MANAGER_CONNECT);
            if (scm == IntPtr.Zero)
            {
                Facade.Logger.Error(string.Format("Failed to open Service Controller Manager. Win32 error {0}.", Marshal.GetLastWin32Error()));
                goto cleanup;
            }

            svc = OpenServiceA(scm, serviceName, SERVICE_STOP);
            if (scm == IntPtr.Zero)
            {
                Facade.Logger.Error(string.Format("Failed to open service {0}. Win32 error {1}.", serviceName, Marshal.GetLastWin32Error()));
                goto cleanup;
            }

            if (!ControlService(svc, SERVICE_CONTROL_STOP, IntPtr.Zero))
            {
                Facade.Logger.Error(string.Format("Failed to stop service {0}. Win32 error {1}.", serviceName, Marshal.GetLastWin32Error()));
            }

cleanup:
            if (svc != IntPtr.Zero)
                CloseServiceHandle(svc);
            if (scm != IntPtr.Zero)
                CloseServiceHandle(scm);
        }
    }
}

using AntiForensicLib;
using Serilog;
using System;

namespace AntiForensicTool
{
    internal class SerilogProxy : LoggerProxy
    {
        public void Verbose(string message) => Log.Verbose(message);
        public void Debug(string message) => Log.Debug(message);
        public void Information(string message) => Log.Information(message);
        public void Warning(string message) => Log.Warning(message);
        public void Error(string message) => Log.Error(message);
        public void Error(Exception ex, string message) => Log.Error(ex, message);
        public void Fatal(string message) => Log.Fatal(message);
        public void Fatal(Exception ex, string message) => Log.Fatal(ex, message);
    }
}

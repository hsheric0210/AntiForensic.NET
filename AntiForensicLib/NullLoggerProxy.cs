using System;

namespace AntiForensicLib
{
    internal class NullLoggerProxy : LoggerProxy
    {
        public void Verbose(string message) { }
        public void Debug(string message) { }
        public void Information(string message) { }
        public void Warning(string message) { }
        public void Error(string message) { }
        public void Error(Exception ex, string message) { }
        public void Fatal(string message) { }
        public void Fatal(Exception ex, string message) { }
    }
}

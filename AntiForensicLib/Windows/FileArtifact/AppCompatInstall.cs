﻿using System;
using System.IO;

namespace AntiForensicLib.Windows.FileArtifact
{
    internal class AppCompatInstall : ICleaner
    {
        public CleanerTypes Type => CleanerTypes.AppCompatInstall;

        public string Name => "AppCompat Installation Logs";

        public int RunCleaner() => FileUtils.EliminateFolderSubitems(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "AppCompat", "Programs", "Install"), "*.txt");
    }
}

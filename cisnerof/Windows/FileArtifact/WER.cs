using System;
using System.IO;

namespace cisnerof.Windows.FileArtifact
{
    /// <summary>
    /// https://yum-history.tistory.com/295
    /// </summary>
    internal class WER : ICleaner
    {
        public CleanerTypes Type => CleanerTypes.WER;

        public string Name => "Windows Error Reporting";

        public int RunCleaner() => FileUtils.EliminateFolderSubitems(Path.Combine(Environment.GetEnvironmentVariable("ProgramData"), "Microsoft", "Windows", "WER", "ReportArchive"));
    }
}

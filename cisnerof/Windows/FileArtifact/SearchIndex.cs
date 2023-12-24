using System;
using System.IO;

namespace cisnerof.Windows.FileArtifact
{
    /// <summary>
    /// https://yum-history.tistory.com/295
    /// </summary>
    internal class SearchIndex : ICleaner
    {
        public CleanerTypes Type => CleanerTypes.SearchIndex;

        public string Name => "Windows Search Index";

        public int RunCleaner()
        {
            ServiceUtils.StopService("WSearch");
            return FileUtils.EliminateFolderSubitems(Path.Combine(Environment.GetEnvironmentVariable("ProgramData"), "Microsoft", "Search", "Data", "Application", "Windows"), "Windows*");
        }
    }
}

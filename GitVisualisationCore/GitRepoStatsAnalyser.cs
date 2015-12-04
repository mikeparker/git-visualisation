using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GitVisualisationCore
{
    public class GitRepoStatsAnalyser
    {
        private readonly string baseFilePath;

        public GitRepoStatsAnalyser(string baseFilePath)
        {
            this.baseFilePath = baseFilePath;
        }

        public IEnumerable<string> GetAllCSharpFilePaths()
        {
            return Directory.EnumerateFiles(baseFilePath, "*.cs", SearchOption.AllDirectories).Where(s => !s.Contains("TemporaryGeneratedFile"));
        }
    }
}
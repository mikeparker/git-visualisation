using System.Collections.Generic;
using System.IO;
using System.Linq;
using LibGit2Sharp;

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

        public IEnumerable<FileStats> GetAllCSharpFilePathsAsObjects()
        {
            var repo = new Repository(@"C:\git\git-visualisation\.git");
            foreach (string f in GetAllCSharpFilePaths())
            {
//                var filter = new CommitFilter { Since = repo.Branches["master"], Until = repo.Branches["development"] };
//                var commits = repo.Commits.QueryBy(filter).ToList();
                var commits = repo.Commits.ToList();

                yield return new FileStats(f, commits.Count);
            }
        }

        public void GetSomething()
        {
            DirectoryInfo DirInfo = new DirectoryInfo(baseFilePath);
        }
    }

    public class FileStats
    {
        private readonly string filepath;
        private readonly int numberOfCommitsInLast5Years;

        public FileStats(string filepath, int numberOfCommitsInLast5Years)
        {
            this.filepath = filepath;
            this.numberOfCommitsInLast5Years = numberOfCommitsInLast5Years;
        }


        public override string ToString()
        {
            return filepath + " " + numberOfCommitsInLast5Years;
        }
    }
}
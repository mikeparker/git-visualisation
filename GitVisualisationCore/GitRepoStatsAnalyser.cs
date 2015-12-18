using System;
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
            var fileStatsByFilepath = new FileStatsCollection();
//            using (var repo = new Repository(@"C:\git\git-visualisation\.git"))
            using (var repo = new Repository(@"C:\Git\michaelparker-mi-6\.git"))
            {
                var commit1 = repo.Head.Tip;
                for (int i = 0; i < 100; i++)
                {
                    var secondCommit = commit1.Parents.FirstOrDefault();
                    if (secondCommit == null)
                    {
                        break;
                    }

                    Tree commitTree = commit1.Tree;
                    Tree parentCommitTree = secondCommit.Tree;

                    var patch = repo.Diff.Compare<Patch>(parentCommitTree, commitTree);
                    
                    Console.WriteLine("Commit."); // Status -> File Path
                    foreach (var ptc in patch)
                    {
                        fileStatsByFilepath.AddCommit(ptc);
                        Console.WriteLine(ptc.Status +" -> "+ptc.Path); // Status -> File Path
                    }

                    commit1 = secondCommit;
                }
            }

            return fileStatsByFilepath.GetValues();
        }
    }

    public class FileStatsCollection
    {
        private readonly Dictionary<string, FileStats> fileStatsByFilepath = new Dictionary<string, FileStats>();

        public void AddCommit(PatchEntryChanges ptc)
        {
            FileStats x;
            if (fileStatsByFilepath.TryGetValue(ptc.Path, out x))
            {
                x.AddCommit(ptc);
            }
            else
            {
                fileStatsByFilepath[ptc.Path] = new FileStats(ptc.Path);
            }
        }

        public IEnumerable<FileStats> GetValues()
        {
            return fileStatsByFilepath.Values.ToList();
        }
    }

    public class FileStats
    {
        private readonly string filepath;
        private int numberOfCommits;

        public FileStats(string filepath)
        {
            this.filepath = filepath;
            numberOfCommits = 1;
        }


        public override string ToString()
        {
            return filepath + " " + numberOfCommits;
        }

        public void AddCommit(PatchEntryChanges ptc)
        {
            numberOfCommits++;
        }
    }
}
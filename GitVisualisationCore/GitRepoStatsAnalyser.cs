using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
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
                for (int i = 0; i < 1000; i++)
                {
                    var secondCommit = commit1.Parents.FirstOrDefault();
                    if (secondCommit == null)
                    {
                        break;
                    }

                    Tree commitTree = commit1.Tree;
                    Tree parentCommitTree = secondCommit.Tree;

                    var patch = repo.Diff.Compare<Patch>(parentCommitTree, commitTree);
                    
                    Console.WriteLine("Commit: " + commit1.Message); // Status -> File Path
                    Match match = Regex.Match(commit1.Message, @"MI-\d+", RegexOptions.IgnoreCase);
                    foreach (var ptc in patch)
                    {
                        fileStatsByFilepath.AddCommit(ptc, commit1, match);
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

        public void AddCommit(PatchEntryChanges ptc, Commit commit1, Match match)
        {
            FileStats fileStats;
            if (fileStatsByFilepath.TryGetValue(ptc.Path, out fileStats))
            {
            }
            else
            {
                fileStats = new FileStats(ptc.Path);
                fileStatsByFilepath[ptc.Path] = fileStats;
            }

            fileStats.AddCommit(ptc, commit1, match);
        }

        public IEnumerable<FileStats> GetValues()
        {
            return fileStatsByFilepath.Values.OrderBy(v => v.NumberOfCommits).ToList();
        }
    }

    public class FileStats
    {
        private readonly string filepath;
        private int numberOfCommits;
        private readonly HashSet<string> bugsFixedInThisPath = new HashSet<string>(); 

        public FileStats(string filepath)
        {
            this.filepath = filepath;
            numberOfCommits = 0;
        }

        public int NumberOfCommits
        {
            get { return numberOfCommits; }
        }


        public override string ToString()
        {
            return filepath + " " + NumberOfCommits + " " + string.Join(", ", bugsFixedInThisPath);
        }

        public void AddCommit(PatchEntryChanges ptc, Commit commit1, Match match)
        {
            numberOfCommits = NumberOfCommits + 1;
            if (!match.Success)
            {
                return;
            }

            foreach (var @group in match.Groups)
            {
                var bugNumber = @group.ToString();
                bugsFixedInThisPath.Add(bugNumber.ToUpper());
            }
        }
    }
}
using System.Collections.Generic;
using System.Text.RegularExpressions;
using LibGit2Sharp;

namespace GitVisualisationCore
{
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
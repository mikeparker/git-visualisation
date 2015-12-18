using System.Diagnostics;
using NUnit.Framework;

namespace GitVisualisationCore.Tests
{
    [TestFixture]
    public class GitRepoStatsAnalyserTests
    {
        [Test]
        public void TestCollectingCurrentFiles()
        {
            var sut = new GitRepoStatsAnalyser(@"C:\git\git-visualisation\");

            var allCSharpFilePaths = sut.GetAllCSharpFilePaths();

            foreach (var file in allCSharpFilePaths)
            {
                Debug.WriteLine(file);
            }
        }

        [Test]
        public void TestCollectingCurrentFilesAsObjects()
        {
            var sut = new GitRepoStatsAnalyser(@"C:\git\git-visualisation\");

            var allCSharpFilePaths = sut.GetAllCSharpFilePathsAsObjects();

            foreach (var file in allCSharpFilePaths)
            {
                Debug.WriteLine(file);
            }
        }
    }
}

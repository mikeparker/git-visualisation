using System;
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
        public void TestNothing()
        {
            Assert.That(8, Is.EqualTo(6+2));
        }
    }
}

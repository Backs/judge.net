using System.IO;
using Judge.Checker;
using NUnit.Framework;

namespace Judge.Tests.Checker.CheckerTests
{
    [TestFixture]
    public class CheckTests
    {
        private readonly string _workingDirectory = Path.Combine(TestContext.CurrentContext.TestDirectory, "WorkingDirectory");

        [Test]
        public void IntCheckerTest()
        {
            CreateFile("output.txt", "4");
            CreateFile("answer.txt", "4");
            CopyChecker("icmp.exe");

            var checker = new Judge.Checker.Checker();

            var result = checker.Check(_workingDirectory, "input.txt", "output.txt", "answer.txt");

            Assert.That(result.CheckStatus, Is.EqualTo(CheckStatus.OK), result.Message);
        }

        [TearDown]
        public void TearDown()
        {
            DeleteFile("input.txt");
            DeleteFile("output.txt");
            DeleteFile("answer.txt");
            DeleteFile("check.exe");
        }

        private void CreateFile(string fileName, string content)
        {
            if (!Directory.Exists(_workingDirectory))
            {
                Directory.CreateDirectory(_workingDirectory);
            }
            var file = Path.Combine(_workingDirectory, fileName);

            File.WriteAllText(file, content);
        }

        private void CopyChecker(string checkerName)
        {
            var source = Path.Combine(TestContext.CurrentContext.TestDirectory, @"TestCheckers", checkerName);
            var dest = Path.Combine(_workingDirectory, "check.exe");
            File.Copy(source, dest);
        }

        private void DeleteFile(string fileName)
        {
            var file = Path.Combine(_workingDirectory, fileName);
            if (File.Exists(file))
            {
                File.Delete(file);
            }
        }
    }
}

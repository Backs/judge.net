using System.IO;
using Judge.Runner;
using NUnit.Framework;

namespace Judge.Tests.Runner.RunServiceTests
{
    [TestFixture]
    public class RunTests
    {
        private readonly string _runnerPath = Path.Combine(TestContext.CurrentContext.TestDirectory, @"run-x64\run.exe");
        private readonly string _workingDirectory = Path.Combine(TestContext.CurrentContext.TestDirectory, "WorkingDirectory");

        [Test]
        public void RunCmdTest()
        {
            var service = new RunService(_runnerPath, _workingDirectory);

            var configuration = new Configuration("cmd", _workingDirectory, 1000, 10 * 1024 * 1024);

            service.Run(configuration);
        }

        [Test]
        public void TimeLimitSolutionTest()
        {
            var service = new RunService(_runnerPath, _workingDirectory);

            var fileName = Path.Combine(TestContext.CurrentContext.TestDirectory, @"TestSolutions\TL.exe");
            var configuration = new Configuration(fileName, _workingDirectory, 100, 10 * 1024 * 1024);

            var result = service.Run(configuration);

            Assert.That(result.RunStatus, Is.EqualTo(RunStatus.TimeLimitExceeded));
        }

        [Test]
        public void IdleSolutionTest()
        {
            var service = new RunService(_runnerPath, _workingDirectory);

            var fileName = Path.Combine(TestContext.CurrentContext.TestDirectory, @"TestSolutions\IdleTest.exe");
            var configuration = new Configuration(fileName, _workingDirectory, 100, 10 * 1024 * 1024);

            var result = service.Run(configuration);

            Assert.That(result.RunStatus, Is.EqualTo(RunStatus.IdlenessLimitExceeded));
        }

        [Test]
        public void MemoryLimitTest()
        {
            var service = new RunService(_runnerPath, _workingDirectory);

            var configuration = new Configuration(@"notepad", _workingDirectory, 1000, 10 * 1024);

            var result = service.Run(configuration);

            Assert.That(result.RunStatus, Is.EqualTo(RunStatus.MemoryLimitExceeded));
        }

        [Test]
        public void InvalidCodeSolutionTest()
        {
            var service = new RunService(_runnerPath, _workingDirectory);

            var fileName = Path.Combine(TestContext.CurrentContext.TestDirectory, @"TestSolutions\InvalidReturnCode.exe");
            var configuration = new Configuration(fileName, _workingDirectory, 1000, 10 * 1024 * 1024);

            var result = service.Run(configuration);

            Assert.That(result.RunStatus, Is.EqualTo(RunStatus.RuntimeError));
        }

        [Test]
        public void RuntimeErrorSolutionTest()
        {
            var service = new RunService(_runnerPath, _workingDirectory);

            var fileName = Path.Combine(TestContext.CurrentContext.TestDirectory, @"TestSolutions\RuntimeError.exe");
            var configuration = new Configuration(fileName, _workingDirectory, 1000, 10 * 1024 * 1024);

            var result = service.Run(configuration);

            Assert.That(result.RunStatus, Is.EqualTo(RunStatus.RuntimeError));
        }

        [Test]
        public void InvocationFailedTest()
        {
            var service = new RunService(_runnerPath, _workingDirectory);

            var fileName = Path.Combine(TestContext.CurrentContext.TestDirectory, @"TestSolutions\AB.exe");
            var configuration = new Configuration(fileName, _workingDirectory, 1000, 10 * 1024 * 1024)
            {
                InputFile = "input.txt",
                OutputFile = "output.txt"
            };

            var result = service.Run(configuration);

            Assert.That(result.RunStatus, Is.EqualTo(RunStatus.InvocationFailed));
        }

        [Test]
        public void UseFilesTest()
        {
            using (var input = CreateFile("input.txt"))
            {
                input.Write("1 2");
            }

            var service = new RunService(_runnerPath, _workingDirectory);

            var fileName = Path.Combine(TestContext.CurrentContext.TestDirectory, @"TestSolutions\AB.exe");
            var configuration = new Configuration(fileName, _workingDirectory, 1000, 10 * 1024 * 1024)
            {
                InputFile = "input.txt",
                OutputFile = "output.txt"
            };

            var result = service.Run(configuration);

            Assert.That(result.RunStatus, Is.EqualTo(RunStatus.Success));
        }

        [TearDown]
        public void TearDown()
        {
            DeleteFile(Path.Combine(_workingDirectory, "input.txt"));
            DeleteFile(Path.Combine(_workingDirectory, "output.txt"));
        }

        private StreamWriter CreateFile(string fileName)
        {
            fileName = Path.Combine(_workingDirectory, fileName);
            DeleteFile(fileName);

            return new StreamWriter(fileName);
        }

        private static void DeleteFile(string fileName)
        {
            if (File.Exists(fileName))
                File.Delete(fileName);
        }
    }
}

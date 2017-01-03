using System.IO;
using Judge.Runner;
using NUnit.Framework;

namespace Judge.Tests.Runner.RunServiceTests
{
    [TestFixture]
    class RunTests
    {
        private readonly string _runnerPath = Path.Combine(TestContext.CurrentContext.TestDirectory, @"run-x64\run.exe");
        private readonly string _workingDirectory = Path.Combine(TestContext.CurrentContext.TestDirectory, "WorkingDirectory");

        [Test]
        public void RunCmdTest()
        {
            var service = new RunService(_runnerPath, _workingDirectory);

            var configuration = new Configuration("cmd", null, 1000, 10 * 1024 * 1024);

            service.Run(configuration);
        }

        [Test]
        public void TimeLimitSolutionTest()
        {
            var service = new RunService(_runnerPath, _workingDirectory);

            var fileName = Path.Combine(TestContext.CurrentContext.TestDirectory, @"TestSolutions\TL.exe");
            var configuration = new Configuration(fileName, null, 1000, 10 * 1024 * 1024);

            var result = service.Run(configuration);

            Assert.That(result.RunStatus, Is.EqualTo(RunStatus.TimeLimitExceeded));
        }

        [Test]
        public void MemoryLimitTest()
        {
            var service = new RunService(_runnerPath, _workingDirectory);

            var configuration = new Configuration(@"notepad", null, 1000, 10 * 1024);

            var result = service.Run(configuration);

            Assert.That(result.RunStatus, Is.EqualTo(RunStatus.MemoryLimitExceeded));
        }

        [Test]
        public void InvalidCodeSolutionTest()
        {
            var service = new RunService(_runnerPath, _workingDirectory);

            var fileName = Path.Combine(TestContext.CurrentContext.TestDirectory, @"TestSolutions\InvalidReturnCode.exe");
            var configuration = new Configuration(fileName, null, 1000, 10 * 1024 * 1024);

            var result = service.Run(configuration);

            Assert.That(result.RunStatus, Is.EqualTo(RunStatus.RuntimeError));
        }

        [Test]
        public void RuntimeErrorSolutionTest()
        {
            var service = new RunService(_runnerPath, _workingDirectory);

            var fileName = Path.Combine(TestContext.CurrentContext.TestDirectory, @"TestSolutions\RuntimeError.exe");
            var configuration = new Configuration(fileName, null, 1000, 10 * 1024 * 1024);

            var result = service.Run(configuration);

            Assert.That(result.RunStatus, Is.EqualTo(RunStatus.RuntimeError));
        }
    }
}

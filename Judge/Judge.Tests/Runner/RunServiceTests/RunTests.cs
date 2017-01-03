using Judge.Runner;
using NUnit.Framework;

namespace Judge.Tests.Runner.RunServiceTests
{
    [TestFixture]
    class RunTests
    {
        private string _runnerPath = @"C:\Develop\judge.net\judge.net\Judge\Judge.Tests\bin\Debug\run-x64\run.exe";
        private string _workingDirectory = @"C:\Develop\judge.net\judge.net\Judge\Judge.Tests\WorkingDirectory";

        [Test]
        public void RunCmdTest()
        {
            var service = new RunService(_runnerPath,
                _workingDirectory);

            var configuration = new Configuration("cmd", null, 1000, 10 * 1024 * 1024);

            service.Run(configuration);
        }

        [Test]
        public void TimeLimitSolutionTest()
        {
            var service = new RunService(_runnerPath, _workingDirectory);

            var configuration = new Configuration(@"C:\Develop\judge.net\judge.net\Judge\Judge.Tests\TestSolutions\TL.exe", null, 1000, 10 * 1024 * 1024);

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

            var configuration = new Configuration(@"C:\Develop\judge.net\judge.net\Judge\Judge.Tests\TestSolutions\InvalidReturnCode.exe", null, 1000, 10 * 1024 * 1024);

            var result = service.Run(configuration);

            Assert.That(result.RunStatus, Is.EqualTo(RunStatus.RuntimeError));
        }
    }
}

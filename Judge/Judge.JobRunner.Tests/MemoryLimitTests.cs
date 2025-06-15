using FluentAssertions;
using Judge.Runner.Abstractions;
using NUnit.Framework;

namespace Judge.JobRunner.Tests;

[TestFixture]
public class MemoryLimitTests : TestBase
{
    [Test]
    [TestCase("50mb.exe", 60 * 1024 * 1024, RunStatus.Success, TestName = "Pass memory limit test")]
    [TestCase("50mb.exe", 50 * 1024 * 1024, RunStatus.MemoryLimitExceeded, TestName = "Fail memory limit test")]
    public void MemoryLimitTest(string fileName, int memoryLimitBytes, RunStatus status)
    {
        this.CopyFile(fileName);
        var options = new RunOptions
        {
            WorkingDirectory = this.WorkingDirectory,
            TimeLimit = TimeSpan.FromSeconds(4),
            MemoryLimitBytes = memoryLimitBytes,
            Executable = fileName,
            Input = "input.txt",
            Output = "output.txt"
        };

        var result = ProcessRunner.RunProcessWithLimits(options);
        result.Status.Should().Be(status);
    }
}
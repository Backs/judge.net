using FluentAssertions;
using NUnit.Framework;

namespace Judge.JobRunner.Tests;

[TestFixture]
public class ProcessRunnerTests : TestBase
{
    private const int MemoryLimitKiloBytes = 100 * 1024 * 1024;

    [Test]
    [TestCase("TimeLimit.exe")]
    [TestCase("Sleep.exe")]
    public void TimeLimitTest(string fileName)
    {
        this.CopyFile(fileName);
        var options = new RunOptions
        {
            WorkingDirectory = this.WorkingDirectory,
            TimeLimit = TimeSpan.FromSeconds(2),
            MemoryLimitBytes = MemoryLimitKiloBytes,
            Executable = fileName,
            Input = "input.txt",
            Output = "output.txt"
        };

        var result = ProcessRunner.RunProcessWithLimits(options);

        result.Status.Should().Be(RunStatus.TimeLimitExceeded);
    }

    [Test]
    public void SleepTest()
    {
        var options = new RunOptions
        {
            WorkingDirectory = this.WorkingDirectory,
            TimeLimit = TimeSpan.FromSeconds(4),
            MemoryLimitBytes = MemoryLimitKiloBytes,
            Executable = "Sleep.exe",
            Input = "input.txt",
            Output = "output.txt"
        };

        var result = ProcessRunner.RunProcessWithLimits(options);
        result.Status.Should().Be(RunStatus.TimeLimitExceeded);
    }

    [Test]
    public void WaitInputTest()
    {
        var options = new RunOptions
        {
            WorkingDirectory = this.WorkingDirectory,
            TimeLimit = TimeSpan.FromSeconds(1),
            MemoryLimitBytes = MemoryLimitKiloBytes,
            Executable = "WaitInput.exe",
            Input = "input.txt",
            Output = "output.txt"
        };

        var result = ProcessRunner.RunProcessWithLimits(options);
        result.Status.Should().Be(RunStatus.TimeLimitExceeded);
    }

    [Test]
    public void AcceptedTest()
    {
        var options = new RunOptions
        {
            WorkingDirectory = this.WorkingDirectory,
            TimeLimit = TimeSpan.FromSeconds(2),
            MemoryLimitBytes = MemoryLimitKiloBytes,
            Executable = "AB.exe",
            Input = "input.txt",
            Output = "output.txt"
        };

        var result = ProcessRunner.RunProcessWithLimits(options);

        result.Status.Should().Be(RunStatus.Success);
    }

    [Test]
    public void DivideByZeroTest()
    {
        var options = new RunOptions
        {
            WorkingDirectory = this.WorkingDirectory,
            TimeLimit = TimeSpan.FromSeconds(1),
            MemoryLimitBytes = MemoryLimitKiloBytes,
            Executable = "DivideByZero.exe",
            Input = "input.txt",
            Output = "output.txt"
        };

        var result = ProcessRunner.RunProcessWithLimits(options);
        result.Status.Should().Be(RunStatus.RuntimeError);
    }
}
using FluentAssertions;
using NUnit.Framework;

namespace Judge.JobRunner.Tests;

[TestFixture]
public class ProcessRunnerTests
{
    private const int MemoryLimitKiloBytes = 100 * 1024 * 1024;

    [Test]
    public void TimeLimitTest()
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), "Examples", "TimeLimit.exe");

        var result = ProcessRunner.RunProcessWithLimits(
            path, "",
            TimeSpan.FromSeconds(2),
            MemoryLimitKiloBytes);
        result.Status.Should().Be(RunStatus.TimeLimitExceeded);
    }

    [Test]
    public void AcceptedTest()
    {
        Action<StreamWriter> input = sw => sw.WriteLine("1 2");
        var path = Path.Combine(Directory.GetCurrentDirectory(), "Examples", "AB.exe");

        var result = ProcessRunner.RunProcessWithLimits(
            path, "1 2",
            TimeSpan.FromSeconds(1),
            MemoryLimitKiloBytes,
            input);
        result.Status.Should().Be(RunStatus.Success);
        result.Output.Should().Be("3");
    }

    [Test]
    public void SleepTest()
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), "Examples", "Sleep.exe");

        var result = ProcessRunner.RunProcessWithLimits(
            path, "",
            TimeSpan.FromSeconds(1),
            MemoryLimitKiloBytes);
        result.Status.Should().Be(RunStatus.TimeLimitExceeded);
    }

    [Test]
    public void WaitInputTest()
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), "Examples", "WaitInput.exe");

        var result = ProcessRunner.RunProcessWithLimits(
            path, "",
            TimeSpan.FromSeconds(1),
            MemoryLimitKiloBytes);
        result.Status.Should().Be(RunStatus.TimeLimitExceeded);
    }

    [Test]
    public void DivideByZeroTest()
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), "Examples", "DivideByZero.exe");

        var result = ProcessRunner.RunProcessWithLimits(
            path, "",
            TimeSpan.FromSeconds(1),
            MemoryLimitKiloBytes);
        result.Status.Should().Be(RunStatus.RuntimeError);
    }
}
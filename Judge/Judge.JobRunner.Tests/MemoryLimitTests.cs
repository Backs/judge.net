using FluentAssertions;
using NUnit.Framework;

namespace Judge.JobRunner.Tests;

[TestFixture]
public class MemoryLimitTests
{
    [Test]
    public void PassMemoryLimitTest()
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), "Examples", "50mb.exe");

        var result = ProcessRunner.RunProcessWithLimits(
            path, "",
            TimeSpan.FromSeconds(10),
            60 * 1024 * 1024);
        result.Status.Should().Be(RunStatus.Success);
    }

    [Test]
    public void FailMemoryLimitTest()
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), "Examples", "50mb.exe");

        var result = ProcessRunner.RunProcessWithLimits(
            path, "",
            TimeSpan.FromSeconds(10),
            50 * 1024 * 1024);
        result.Status.Should().Be(RunStatus.MemoryLimitExceeded);
    }
}
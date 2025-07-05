using FluentAssertions;
using Judge.Runner.Abstractions;
using NUnit.Framework;

namespace Judge.JobRunner.Tests;

[TestFixture]
public class RuntimeErrorTests : TestBase
{
    private const int MemoryLimitKiloBytes = 100 * 1024 * 1024;

    [Test]
    [TestCase("DivideByZero.exe")]
    public void DivideByZeroTest(string fileName)
    {
        this.CopyFile(fileName);
        var options = new RunOptions
        {
            WorkingDirectory = this.WorkingDirectory,
            TimeLimit = TimeSpan.FromSeconds(1),
            MemoryLimitBytes = MemoryLimitKiloBytes,
            Executable = fileName,
            Input = "input.txt",
            Output = "output.txt"
        };

        var result = ProcessRunner.RunProcessWithLimits(options);
        result.Status.Should().Be(RunStatus.RuntimeError);
    }
}
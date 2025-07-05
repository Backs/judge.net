using FluentAssertions;
using Judge.Runner.Abstractions;
using NUnit.Framework;

namespace Judge.JobRunner.Tests;

[TestFixture]
public class AcceptedTests : TestBase
{
    private const int MemoryLimitKiloBytes = 100 * 1024 * 1024;

    protected override void SetUp()
    {
        File.WriteAllText(Path.Combine(this.WorkingDirectory, "input.txt"), "1 2");
    }

    [Test]
    public void ABTest()
    {
        const string fileName = "AB.exe";

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

        result.Status.Should().Be(RunStatus.Success);

        var output = File.ReadAllText(Path.Combine(this.WorkingDirectory, "output.txt"))
            .Trim();

        output.Should().Be("3");
    }
}
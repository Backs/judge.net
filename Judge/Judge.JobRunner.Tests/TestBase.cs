using NUnit.Framework;

namespace Judge.JobRunner.Tests;

public abstract class TestBase
{
    protected string WorkingDirectory = null!;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        this.WorkingDirectory = Path.Combine(Directory.GetCurrentDirectory(), "work");
    }

    [SetUp]
    public void SetUp()
    {
        Directory.CreateDirectory(this.WorkingDirectory);
        File.WriteAllText(Path.Combine(this.WorkingDirectory, "input.txt"), "test");
    }

    [TearDown]
    public void TearDown()
    {
        Directory.Delete(this.WorkingDirectory, true);
    }

    protected void CopyFile(string fileName)
    {
        var file = Path.Combine(Directory.GetCurrentDirectory(), "Examples", fileName);
        File.Copy(file, Path.Combine(this.WorkingDirectory, fileName));
    }
}
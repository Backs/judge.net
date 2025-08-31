using NUnit.Framework;

namespace Judge.JobRunner.Tests;

[Explicit]
public abstract class TestBase
{
    protected string WorkingDirectory = null!;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        this.WorkingDirectory = Path.Combine(Directory.GetCurrentDirectory(), "work");
    }

    [SetUp]
    public void SetUpInternal()
    {
        if (Directory.Exists(this.WorkingDirectory))
        {
            Directory.Delete(this.WorkingDirectory, true);
        }

        Directory.CreateDirectory(this.WorkingDirectory);
        this.SetUp();
    }

    protected virtual void SetUp()
    {
        File.Create(Path.Combine(this.WorkingDirectory, "input.txt")).Dispose();
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
using Judge.Runner.Abstractions;

namespace Judge.Runner;

public interface IRunService
{
    IRunResult Run(RunOptions options);
}
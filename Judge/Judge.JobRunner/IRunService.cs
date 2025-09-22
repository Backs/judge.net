using Judge.Runner.Abstractions;

namespace Judge.JobRunner;

public interface IRunService
{
    IRunResult Run(RunOptions options);
}
using Judge.Runner.Abstractions;

namespace Judge.JobRunner;

public sealed class JobObjectRunner : IRunService
{
    public IRunResult Run(RunOptions options)
    {
        return ProcessRunner.RunProcessWithLimits(options);
    }
}
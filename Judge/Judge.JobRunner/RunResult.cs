using System;

namespace Judge.JobRunner;

public class RunResult
{
    public int ExitCode { get; init; }
    public RunStatus Status { get; init; }
    public TimeSpan TimeConsumed { get; init; }
    public int PeakMemoryUsed { get; init; }
}
namespace Judge.Runner.Abstractions;

public class RunResult : IRunResult
{
    public int ExitCode { get; init; }
    public RunStatus Status { get; init; }
    public int TimeConsumedMilliseconds { get; init; }
    public int PeakMemoryUsed { get; init; }
    public string Description { get; init; }
    public string Output { get; init; }
    public string TextStatus { get; init; }
}
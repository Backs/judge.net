namespace Judge.Runner.Abstractions;

public interface IRunResult
{
    int ExitCode { get; }
    RunStatus Status { get; }
    int TimeConsumedMilliseconds { get; }
    int PeakMemoryUsed { get; }
    string Description { get; }
    string Output { get; }
    string TextStatus { get; }
}
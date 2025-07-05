using System;
using System.Globalization;
using System.Text.RegularExpressions;
using Judge.Runner.Abstractions;

namespace Judge.Runner;

public sealed class RunResult : IRunResult
{
    public int ExitCode { get; private init; }
    public RunStatus Status { get; private init; }
    public TimeSpan TimeConsumed => TimeSpan.FromMilliseconds(this.TimeConsumedMilliseconds);
    public int TimeConsumedMilliseconds { get; private init; }
    public int TimePassedMilliseconds { get; private set; }
    public int PeakMemoryUsed { get; private init; }
    public string TextStatus { get; private set; }
    public string Description { get; private set; }
    public string Output { get; private set; }

    private RunResult()
    {
    }

    private static readonly Regex TimeRegex = new Regex(@"\d*\.\d{2}", RegexOptions.Compiled);
    private static readonly Regex NumberRegex = new Regex(@"\d+", RegexOptions.Compiled);

    public static RunResult Parse(string input, int exitCode)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            throw new ArgumentOutOfRangeException(nameof(input));
        }

        var rows = input.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
        var startIndex = Array.FindIndex(rows, o => o.StartsWith("  time consumed"));

        var textStatus = rows[1];
        var runStatus = GetStatus(textStatus, exitCode);

        if (runStatus == RunStatus.InvocationFailed)
        {
            return InvocationFailed(textStatus, input);
        }

        var description = startIndex == 2 ? rows[1] : rows[2];
        var timeConsumedRow = rows[startIndex];
        var timePassedRow = rows[startIndex + 1];
        var peakMemoryRow = rows[startIndex + 2];

        var runResult = new RunResult
        {
            TimeConsumedMilliseconds =
                (int)
                (double.Parse(TimeRegex.Match(timeConsumedRow).Value, CultureInfo.InvariantCulture) * 1000),
            TimePassedMilliseconds =
                (int)(double.Parse(TimeRegex.Match(timePassedRow).Value, CultureInfo.InvariantCulture) * 1000),
            PeakMemoryUsed = int.Parse(NumberRegex.Match(peakMemoryRow).Value),
            Status = runStatus,
            TextStatus = textStatus,
            Description = description,
            Output = input,
            ExitCode = exitCode
        };
        return runResult;
    }

    private static RunResult InvocationFailed(string textStatus, string output)
    {
        return new RunResult
        {
            Status = RunStatus.InvocationFailed,
            TextStatus = textStatus,
            Output = output
        };
    }

    private static RunStatus GetStatus(string textStatus, int exitCode)
    {
        switch (textStatus)
        {
            case "Time limit exceeded":
                return RunStatus.TimeLimitExceeded;
            case "Memory limit exceeded":
                return RunStatus.MemoryLimitExceeded;
            case "Crash":
                return RunStatus.RuntimeError;
            case "Idleness limit exceeded":
                return RunStatus.IdlenessLimitExceeded;
        }

        if (textStatus.StartsWith("Security violation"))
            return RunStatus.SecurityViolation;

        if (textStatus.StartsWith("Invocation failed"))
            return RunStatus.InvocationFailed;

        if (exitCode != 0)
        {
            return RunStatus.RuntimeError;
        }

        if (textStatus.StartsWith("Program successfully terminated"))
            return RunStatus.Success;

        throw new ArgumentOutOfRangeException(nameof(textStatus));
    }
}
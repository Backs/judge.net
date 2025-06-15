using System;
using Judge.Checker;
using Judge.Compiler;
using Judge.Model.SubmitSolution;
using Judge.Runner.Abstractions;

namespace Judge.JudgeService;

internal sealed class JudgeResult
{
    public CompileResult CompileResult { get; set; }
    public RunStatus? RunStatus { get; set; }
    public int? TimeConsumedMilliseconds { get; set; }
    public int? PeakMemoryBytes { get; set; }
    public string TextStatus { get; set; }
    public string Description { get; set; }
    public string Output { get; set; }
    public int TestRunsCount { get; set; }

    public int TestsPassedCount =>
        this.GetStatus() == SubmitStatus.Accepted ? this.TestRunsCount : this.TestRunsCount - 1;

    public CheckStatus? CheckStatus { get; set; }
    public int? TimePassedMilliseconds { get; set; }

    public SubmitStatus GetStatus()
    {
        if (this.CompileResult.CompileStatus == CompileStatus.CompilerNotFound)
        {
            return SubmitStatus.ServerError;
        }

        if (this.CompileResult.CompileStatus == CompileStatus.Error)
        {
            return SubmitStatus.CompilationError;
        }

        if (this.RunStatus == null)
            throw new InvalidOperationException();

        return this.RunStatus.Value switch
        {
            Judge.Runner.Abstractions.RunStatus.TimeLimitExceeded => SubmitStatus.TimeLimitExceeded,
            Judge.Runner.Abstractions.RunStatus.MemoryLimitExceeded => SubmitStatus.MemoryLimitExceeded,
            Judge.Runner.Abstractions.RunStatus.SecurityViolation => SubmitStatus.RuntimeError,
            Judge.Runner.Abstractions.RunStatus.RuntimeError => SubmitStatus.RuntimeError,
            Judge.Runner.Abstractions.RunStatus.InvocationFailed => SubmitStatus.ServerError,
            Judge.Runner.Abstractions.RunStatus.IdlenessLimitExceeded => SubmitStatus.TimeLimitExceeded,
            Judge.Runner.Abstractions.RunStatus.Success => this.GetCheckStatus(),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private SubmitStatus GetCheckStatus()
    {
        if (this.CheckStatus == null)
            throw new InvalidOperationException();

        return this.CheckStatus.Value switch
        {
            Checker.CheckStatus.OK => SubmitStatus.Accepted,
            Checker.CheckStatus.PE => SubmitStatus.PresentationError,
            Checker.CheckStatus.Fail => SubmitStatus.ServerError,
            Checker.CheckStatus.TooEarly => SubmitStatus.TooEarly,
            Checker.CheckStatus.Unpolite => SubmitStatus.Unpolite,
            Checker.CheckStatus.TooManyLines => SubmitStatus.TooManyLines,
            Checker.CheckStatus.WrongLanguage => SubmitStatus.WrongLanguage,
            Checker.CheckStatus.PullRequestNotFound => SubmitStatus.PullRequestNotFound,
            Checker.CheckStatus.LoginNotFound => SubmitStatus.LoginNotFound,
            Checker.CheckStatus.NotSolvedYet => SubmitStatus.NotSolvedYet,
            _ => SubmitStatus.WrongAnswer
        };
    }
}
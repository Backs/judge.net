using System;
using Judge.Checker;
using Judge.Compiler;
using Judge.Model.SubmitSolution;
using Judge.Runner;

namespace Judge.JudgeService
{
    internal sealed class JudgeResult
    {
        public CompileResult CompileResult { get; set; }
        public RunStatus RunStatus { get; set; }
        public int TimeConsumedMilliseconds { get; set; }
        public int PeakMemoryBytes { get; set; }
        public string TextStatus { get; set; }
        public string Description { get; set; }
        public string Output { get; set; }
        public int TestRunsCount { get; set; }
        public int TestsPassedCount => GetStatus() == SubmitStatus.Accepted ? TestRunsCount : TestRunsCount - 1;
        public CheckStatus CheckStatus { get; set; }

        public SubmitStatus GetStatus()
        {
            if (CompileResult.CompileStatus == CompileStatus.Error)
            {
                return SubmitStatus.CompilationError;
            }
            switch (RunStatus)
            {
                case RunStatus.TimeLimitExceeded:
                    return SubmitStatus.TimeLimitExceeded;
                case RunStatus.MemoryLimitExceeded:
                    return SubmitStatus.MemoryLimitExceeded;
                case RunStatus.SecurityViolation:
                    return SubmitStatus.RuntimeError;
                case RunStatus.RuntimeError:
                    return SubmitStatus.RuntimeError;
                case RunStatus.InvocationFailed:
                    return SubmitStatus.ServerError;
                case RunStatus.IdlenessLimitExceeded:
                    return SubmitStatus.TimeLimitExceeded;
                case RunStatus.Success:
                    return GetCheckStatus();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private SubmitStatus GetCheckStatus()
        {
            switch (CheckStatus)
            {
                case CheckStatus.OK:
                    return SubmitStatus.Accepted;
                case CheckStatus.Fail:
                    return SubmitStatus.ServerError;
                default:
                    return SubmitStatus.WrongAnswer;
            }
        }
    }
}

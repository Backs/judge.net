using System;

namespace Judge.Model.SubmitSolution
{
    public static class SubmitStatusExtensions
    {
        public static string GetDescription(this SubmitStatus submitStatus)
        {
            switch (submitStatus)
            {
                case SubmitStatus.Pending:
                    return "Pending...";
                case SubmitStatus.CompilationError:
                    return "Compilation error";
                case SubmitStatus.RuntimeError:
                    return "Runtime error";
                case SubmitStatus.TimeLimitExceeded:
                    return "Time limit exceeded";
                case SubmitStatus.MemoryLimitExceeded:
                    return "Memory limit exceeded";
                case SubmitStatus.WrongAnswer:
                    return "Wrong answer";
                case SubmitStatus.Accepted:
                    return "Accepted";
                case SubmitStatus.ServerError:
                    return "Server error";
            }
            throw new ArgumentOutOfRangeException();
        }
    }
}

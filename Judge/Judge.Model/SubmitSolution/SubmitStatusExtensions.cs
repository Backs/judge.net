namespace Judge.Model.SubmitSolution
{
    using System;

    public static class SubmitStatusExtensions
    {
        public static string GetDescription(this SubmitStatus submitStatus)
        {
            return submitStatus switch
            {
                SubmitStatus.Pending => "Pending...",
                SubmitStatus.CompilationError => "Compilation error",
                SubmitStatus.RuntimeError => "Runtime error",
                SubmitStatus.TimeLimitExceeded => "Time limit exceeded",
                SubmitStatus.MemoryLimitExceeded => "Memory limit exceeded",
                SubmitStatus.WrongAnswer => "Wrong answer",
                SubmitStatus.Accepted => "Accepted",
                SubmitStatus.ServerError => "Server error",
                
                SubmitStatus.TooEarly => "Too early submit",
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}

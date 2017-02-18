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
        public int PassedTests { get; set; }

        public SubmitStatus GetStatus()
        {
            if (CompileResult.CompileStatus == CompileStatus.Error)
            {
                return SubmitStatus.CompilationError;
            }

            return SubmitStatus.WrongAnswer;
        }
    }
}

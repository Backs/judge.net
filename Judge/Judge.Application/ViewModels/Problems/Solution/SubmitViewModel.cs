using Judge.Model.SubmitSolution;

namespace Judge.Application.ViewModels.Problems.Solution
{
    public sealed class SubmitViewModel
    {
        public int? PassedTests { get; set; }
        public SubmitStatus Status { get; set; }
        public string CompileOutput { get; set; }
        public string RunDescription { get; set; }
        public string RunOutput { get; set; }

        public SubmitViewModel(int? totalBytes, int? totalMilliseconds)
        {
            AllocatedMemory = (totalBytes / (1024f * 1024f))?.ToString("F3");
            ExecutionTime = (totalMilliseconds / 1000f)?.ToString("F3");
        }

        public string ExecutionTime { get; }

        public string AllocatedMemory { get; }
        public string UserHost { get; set; }
        public string SessionId { get; set; }

        public string GetStatusDescription()
        {
            return Status.GetDescription();
        }
    }
}

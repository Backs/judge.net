using System;
using Judge.Model.CheckSolution;
using Judge.Model.SubmitSolution;

namespace Judge.Application.ViewModels.Submit
{
    public class SubmitQueueItem
    {
        public SubmitQueueItem(SubmitResult result, string language, Task task, string userName)
        {
            var totalBytes = result.TotalBytes != null ? Math.Min(result.TotalBytes.Value, task.MemoryLimitBytes) : (int?)null;
            var totalMilliseconds = result.TotalMilliseconds != null ? Math.Min(result.TotalMilliseconds.Value, task.TimeLimitMilliseconds) : (int?)null;

            SubmitId = result.Submit.Id;
            Language = language;
            ProblemId = result.Submit.ProblemId;
            ProblemName = task.Name;
            ResultDescription = result.Status.GetDescription();
            SubmitResultId = result.Id;
            SubmitTime = result.Submit.SubmitDateUtc;
            UserId = result.Submit.UserId;
            UserName = userName;
            Status = result.Status;
            CompileResult = result.CompileOutput;

            if (result.Status != SubmitStatus.CompilationError && result.Status != SubmitStatus.ServerError)
            {
                AllocatedMemory = (totalBytes / (1024f * 1024f))?.ToString("F3");
                ExecutionTime = (totalMilliseconds / 1000f)?.ToString("F3");
                if (result.Status != SubmitStatus.Accepted)
                {
                    PassedTests = result.PassedTests;
                }
            }
        }

        public bool Solved => Status == SubmitStatus.Accepted;
        public bool Pending => Status == SubmitStatus.Pending;

        public bool ResultsEnabled { get; set; }
        public long SubmitId { get; }
        private SubmitStatus Status { get; }
        public DateTime SubmitTime { get; }
        public long SubmitResultId { get; }
        public long ProblemId { get; }
        public string ProblemName { get; }
        public long UserId { get; }
        public string UserName { get; }
        public string Language { get; }
        public int? PassedTests { get; }
        public string ResultDescription { get; }
        public string ExecutionTime { get; }
        public string AllocatedMemory { get; }
        public string CompileResult { get; }
        public bool ShowAdditionalResults => ResultsEnabled && Status == SubmitStatus.CompilationError;
    }
}

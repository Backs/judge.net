namespace Judge.Application.ViewModels.Submit
{
    using System;
    using Judge.Model.CheckSolution;
    using Judge.Model.SubmitSolution;

    public class SubmitQueueItem
    {
        public SubmitQueueItem(SubmitResult result, string language, Task task, string userName)
        {
            var totalBytes = result.TotalBytes != null ? Math.Min(result.TotalBytes.Value, task.MemoryLimitBytes) : (int?)null;
            var totalMilliseconds = result.TotalMilliseconds != null ? Math.Min(result.TotalMilliseconds.Value, task.TimeLimitMilliseconds) : (int?)null;

            this.SubmitId = result.Submit.Id;
            this.Language = language;
            this.ProblemId = result.Submit.ProblemId;
            this.ProblemName = task.Name;
            this.ResultDescription = result.Status.GetDescription();
            this.SubmitResultId = result.Id;
            this.SubmitTime = result.Submit.SubmitDateUtc;
            this.UserId = result.Submit.UserId;
            this.UserName = userName;
            this.Status = result.Status;
            this.CompileResult = result.CompileOutput;

            if (result.Status != SubmitStatus.CompilationError && result.Status != SubmitStatus.ServerError)
            {
                this.AllocatedMemory = (totalBytes / (1024f * 1024f))?.ToString("F3");
                this.ExecutionTime = (totalMilliseconds / 1000f)?.ToString("F3");
                if (result.Status != SubmitStatus.Accepted)
                {
                    this.PassedTests = result.PassedTests;
                }
            }
        }

        public bool Solved => this.Status == SubmitStatus.Accepted;
        public bool Pending => this.Status == SubmitStatus.Pending;

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
        public bool ShowAdditionalResults => this.ResultsEnabled && this.Status == SubmitStatus.CompilationError;
    }
}

using System;
using Judge.Model.SubmitSolution;

namespace Judge.Application.ViewModels.Submit
{
    public class SubmitQueueItem
    {
        public SubmitQueueItem(SubmitResult submitResult, string language, string problemName, string userName)
        {
            SubmitId = submitResult.Submit.Id;
            Language = language;
            ProblemId = submitResult.Submit.ProblemId;
            ProblemName = problemName;
            ResultDescription = submitResult.Status.GetDescription();
            SubmitResultId = submitResult.Id;
            SubmitTime = submitResult.Submit.SubmitDateUtc;
            UserId = submitResult.Submit.UserId;
            UserName = userName;
            Status = submitResult.Status;
            CompileResult = submitResult.CompileOutput;

            if (submitResult.Status != SubmitStatus.CompilationError && submitResult.Status != SubmitStatus.ServerError)
            {
                AllocatedMemory = (submitResult.TotalBytes / (1024f * 1024f))?.ToString("F3");
                ExecutionTime = (submitResult.TotalMilliseconds / 1000f)?.ToString("F3");
                if (submitResult.Status != SubmitStatus.Accepted)
                {
                    PassedTests = submitResult.PassedTests;
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

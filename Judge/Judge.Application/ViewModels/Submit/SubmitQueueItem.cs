using System;
using Judge.Model.SubmitSolution;

namespace Judge.Application.ViewModels.Submit
{
    public sealed class SubmitQueueItem
    {
        public SubmitQueueItem(SubmitResult submitResult, string language)
        {
            SubmitId = submitResult.Submit.Id;
            Language = language;
            ProblemId = submitResult.Submit.ProblemId;
            ProblemName = "problem name"; //TODO
            ResultDescription = submitResult.Status.ToString();
            SubmitResultId = submitResult.Id;
            SubmitTime = submitResult.Submit.SubmitDateUtc;
            UserId = submitResult.Submit.UserId;
            UserName = "user name"; //TODO
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

        public long SubmitId { get;}
        public SubmitStatus Status { get; }
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
        public bool ShowAdditionalResults => Status == SubmitStatus.CompilationError;
    }
}

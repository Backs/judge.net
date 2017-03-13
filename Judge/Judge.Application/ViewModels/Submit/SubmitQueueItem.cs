using System;
using Judge.Model.SubmitSolution;

namespace Judge.Application.ViewModels.Submit
{
    public sealed class SubmitQueueItem
    {
        public SubmitQueueItem(SubmitResult submitResult, string language)
        {
            Language = language;
            PassedTests = submitResult.PassedTests;
            ProblemId = submitResult.Submit.ProblemId;
            ProblemName = "problem name"; //TODO
            ResultDescription = submitResult.Status.ToString();
            SubmitResultId = submitResult.Id;
            SubmitTime = submitResult.Submit.SubmitDateUtc;
            AllocatedMemory = (submitResult.TotalBytes / (1024f * 1024f))?.ToString("F3");
            ExecutionTime = (submitResult.TotalMilliseconds / 1000f)?.ToString("F3");
            UserId = submitResult.Submit.UserId;
            UserName = "user name"; //TODO
        }

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
    }
}

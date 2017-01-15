using System;
using Judge.Model.SubmitSolution;

namespace Judge.Application.ViewModels.Submit
{
    public sealed class SubmitQueueItem
    {
        public DateTime SubmitTime { get; set; }
        public long SubmitResultId { get; set; }
        public SubmitStatus Status { get; set; }
        public long ProblemId { get; set; }
        public string ProblemName { get; set; }
        public long UserId { get; set; }
        public string UserName { get; set; }
        public string Language { get; set; }
        public int? TotalMilliseconds { get; set; }
        public int? TotalBytes { get; set; }
        public int? PassedTests { get; set; }

        public string ResultDescription => $"{Status}";
    }
}

using System;

namespace Judge.Web.Client.Submits;

public class SubmitResultInfo
{
    public long SubmitId { get; set; }
    public DateTime SubmitDate { get; set; }
    public string Language { get; set; } = null!;
    public SubmitStatus Status { get; set; }
    public int? PassedTests { get; set; }
    public long? TotalMilliseconds { get; set; }
    public long? TotalBytes { get; set; }
    public long ProblemId { get; set; }
    public string ProblemName { get; set; } = null!;
    public long UserId { get; set; }
    public string UserName { get; set; } = null!;
    public SubmitResultContestInfo? ContestInfo { get; set; }
}
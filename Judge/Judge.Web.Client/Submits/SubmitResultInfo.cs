using System;

namespace Judge.Web.Client.Submits;

/// <summary>
/// Submit result information
/// </summary>
public class SubmitResultInfo
{
    /// <summary>
    /// Submit result id
    /// </summary>
    public long SubmitResultId { get; set; }

    /// <summary>
    /// Submit date
    /// </summary>
    public DateTime SubmitDate { get; set; }

    /// <summary>
    /// Language name
    /// </summary>
    public string Language { get; set; } = null!;

    /// <summary>
    /// Submit status
    /// </summary>
    public SubmitStatus Status { get; set; }

    /// <summary>
    /// Total tests passed
    /// </summary>
    public int? PassedTests { get; set; }

    /// <summary>
    /// Max execution time in milliseconds
    /// </summary>
    public long? TotalMilliseconds { get; set; }

    /// <summary>
    /// Max memory consumed in bytes
    /// </summary>
    public long? TotalBytes { get; set; }

    /// <summary>
    /// Problem id
    /// </summary>
    public long ProblemId { get; set; }

    /// <summary>
    /// Problem name
    /// </summary>
    public string ProblemName { get; set; } = null!;

    /// <summary>
    /// User id
    /// </summary>
    public long UserId { get; set; }

    /// <summary>
    /// User name
    /// </summary>
    public string UserName { get; set; } = null!;

    /// <inheritdoc cref="Judge.Web.Client.Submits.SubmitResultContestInfo"/>
    public SubmitResultContestInfo? ContestInfo { get; set; }
}
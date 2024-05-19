using System;
using System.ComponentModel.DataAnnotations;

namespace Judge.Web.Client.Submits;

/// <summary>
/// Submit result information
/// </summary>
public class SubmitResultInfo
{
    /// <summary>
    /// Submit result id
    /// </summary>
    [Required]
    public long SubmitResultId { get; set; }

    /// <summary>
    /// Submit date
    /// </summary>
    [Required]
    public DateTime SubmitDate { get; set; }

    /// <summary>
    /// Language name
    /// </summary>
    [Required]
    public string Language { get; set; } = null!;

    /// <summary>
    /// Submit status
    /// </summary>
    [Required]
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
    [Required]
    public long ProblemId { get; set; }

    /// <summary>
    /// Problem name
    /// </summary>
    [Required]
    public string ProblemName { get; set; } = null!;

    /// <summary>
    /// User id
    /// </summary>
    [Required]
    public long UserId { get; set; }

    /// <summary>
    /// User name
    /// </summary>
    [Required]
    public string UserName { get; set; } = null!;

    /// <inheritdoc cref="Judge.Web.Client.Submits.SubmitResultContestInfo"/>
    public SubmitResultContestInfo? ContestInfo { get; set; }
}
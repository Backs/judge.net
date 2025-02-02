using System;
using System.ComponentModel.DataAnnotations;

namespace Judge.Web.Client.Contests;

/// <summary>
/// Contest
/// </summary>
public class Contest : ContestInfo
{
    /// <inheritdoc cref="Judge.Web.Client.Contests.ContestTask"/>
    [Required]
    public ContestTask[] Tasks { get; set; } = Array.Empty<ContestTask>();
}
using System;
using System.ComponentModel.DataAnnotations;

namespace Judge.Web.Client.Contests;

/// <summary>
/// List of contests
/// </summary>
public class ContestsInfoList
{
    /// <inheritdoc cref="Judge.Web.Client.Contests.ContestInfo"/>
    [Required]
    public ContestInfo[] Items { get; set; } = Array.Empty<ContestInfo>();

    /// <summary>
    /// Total contests count
    /// </summary>
    [Required]
    public int TotalCount { get; set; }
}
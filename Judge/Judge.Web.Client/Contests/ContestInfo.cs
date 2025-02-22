using System;
using System.ComponentModel.DataAnnotations;

namespace Judge.Web.Client.Contests;

/// <summary>
/// Contest information
/// </summary>
public class ContestInfo
{
    /// <summary>
    /// Contest id
    /// </summary>
    [Required]
    public int Id { get; set; }

    /// <summary>
    /// Contest name
    /// </summary>
    [Required]
    public string Name { get; set; } = null!;

    /// <summary>
    /// Contest start date in UTC
    /// </summary>
    [Required]
    public DateTime StartDate { get; set; }

    /// <summary>
    /// Contest duration
    /// </summary>
    [Required]
    public TimeSpan Duration { get; set; }

    /// <inheritdoc cref="Judge.Web.Client.Contests.ContestStatus"/>
    [Required]
    public ContestStatus Status { get; set; }

    /// <inheritdoc cref="Judge.Web.Client.Contests.ContestRules"/>
    [Required]
    public ContestRules Rules { get; set; }

    /// <summary>
    /// Is opened
    /// </summary>
    [Required]
    public bool IsOpened { get; set; }
}
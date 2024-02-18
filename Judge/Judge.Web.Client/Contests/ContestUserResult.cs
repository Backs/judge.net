using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Judge.Web.Client.Contests;

/// <summary>
/// Contest user results
/// </summary>
public class ContestUserResult
{
    /// <summary>
    /// User position
    /// </summary>
    [Required]
    public int Position { get; set; }

    /// <summary>
    /// User name
    /// </summary>
    [Required]
    public string UserName { get; set; } = null!;

    /// <summary>
    /// User id
    /// </summary>
    [Required]
    public long UserId { get; set; }

    /// <inheritdoc cref="Judge.Web.Client.Contests.ContestUserResult"/>
    [Required]
    public Dictionary<string, ContestProblemResult> Tasks { get; set; } = new();

    /// <summary>
    /// Total solved tasks
    /// </summary>
    [Required]
    public int SolvedCount { get; set; }

    /// <summary>
    /// Total points
    /// </summary>
    [Required]
    public int Points { get; set; }
}
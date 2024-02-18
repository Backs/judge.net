using System;
using System.ComponentModel.DataAnnotations;

namespace Judge.Web.Client.Contests;

/// <summary>
/// Contest problem result
/// </summary>
public class ContestProblemResult
{
    /// <summary>
    /// Task points
    /// </summary>
    [Required]
    public int Points { get; set; }

    /// <summary>
    /// Total problem attempts
    /// </summary>
    [Required]
    public int Attempts { get; set; }

    /// <summary>
    /// Problem solve time
    /// </summary>
    public TimeSpan? Time { get; set; }

    /// <summary>
    /// Problem solved by user
    /// </summary>
    [Required]
    public bool Solved { get; set; }
}
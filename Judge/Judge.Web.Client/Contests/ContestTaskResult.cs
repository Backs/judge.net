using System;
using System.ComponentModel.DataAnnotations;

namespace Judge.Web.Client.Contests;

/// <summary>
/// Contest task result
/// </summary>
public class ContestTaskResult
{
    /// <summary>
    /// Task points
    /// </summary>
    [Required]
    public int Points { get; set; }

    /// <summary>
    /// Total task attempts
    /// </summary>
    [Required]
    public int Attempts { get; set; }

    /// <summary>
    /// Task solve time
    /// </summary>
    public TimeSpan? Time { get; set; }

    /// <summary>
    /// Task solved
    /// </summary>
    [Required]
    public bool Solved { get; set; }
}
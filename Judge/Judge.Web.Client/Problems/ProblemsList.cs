using System;
using System.ComponentModel.DataAnnotations;

namespace Judge.Web.Client.Problems;

/// <summary>
/// Problem list
/// </summary>
public class ProblemsList
{
    /// <summary>
    /// Problems info
    /// </summary>
    [Required]
    public ProblemInfo[] Items { get; set; } = Array.Empty<ProblemInfo>();

    /// <summary>
    /// Total count of problems
    /// </summary>
    [Required]
    public int TotalCount { get; set; }
}
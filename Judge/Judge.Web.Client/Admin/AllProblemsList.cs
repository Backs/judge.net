using System;
using System.ComponentModel.DataAnnotations;

namespace Judge.Web.Client.Admin;

/// <summary>
/// Problem list
/// </summary>
public sealed class AllProblemsList
{
    /// <summary>
    /// Problems info
    /// </summary>
    [Required]
    public AllProblemInfo[] Items { get; set; } = Array.Empty<AllProblemInfo>();

    /// <summary>
    /// Total count of problems
    /// </summary>
    [Required]
    public int TotalCount { get; set; }
}
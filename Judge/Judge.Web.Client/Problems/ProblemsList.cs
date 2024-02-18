using System;

namespace Judge.Web.Client.Problems;

/// <summary>
/// Problem list
/// </summary>
public class ProblemsList
{
    /// <summary>
    /// Problems info
    /// </summary>
    public ProblemInfo[] Items { get; set; } = Array.Empty<ProblemInfo>();

    /// <summary>
    /// Total count of problems
    /// </summary>
    public int TotalCount { get; set; }
}
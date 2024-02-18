using System;

namespace Judge.Web.Client.Submits;

/// <summary>
/// Submit results list
/// </summary>
public class SubmitResultsList
{
    /// <summary>
    /// Submit results
    /// </summary>
    public SubmitResultInfo[] Items { get; set; } = Array.Empty<SubmitResultInfo>();
    
    /// <summary>
    /// Total count of submit results
    /// </summary>
    public int TotalCount { get; set; }
}
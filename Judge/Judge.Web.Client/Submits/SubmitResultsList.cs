using System;
using System.ComponentModel.DataAnnotations;

namespace Judge.Web.Client.Submits;

/// <summary>
/// Submit results list
/// </summary>
public class SubmitResultsList
{
    /// <summary>
    /// Submit results
    /// </summary>
    [Required]
    public SubmitResultInfo[] Items { get; set; } = Array.Empty<SubmitResultInfo>();
    
    /// <summary>
    /// Total count of submit results
    /// </summary>
    [Required]
    public int TotalCount { get; set; }
}
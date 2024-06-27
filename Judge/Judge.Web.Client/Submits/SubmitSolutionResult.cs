using System.ComponentModel.DataAnnotations;

namespace Judge.Web.Client.Submits;

/// <summary>
/// Submit solution result
/// </summary>
public class SubmitSolutionResult
{
    /// <summary>
    /// Submit id
    /// </summary>
    [Required]
    public long Id { get; set; }
}
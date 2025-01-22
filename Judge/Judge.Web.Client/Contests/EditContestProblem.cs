using System.ComponentModel.DataAnnotations;

namespace Judge.Web.Client.Contests;

/// <summary>
/// Contest problem
/// </summary>
public class EditContestProblem
{
    /// <summary>
    /// Problem id
    /// </summary>
    [Required]
    public long ProblemId { get; set; }
    
    /// <summary>
    /// Problem label in contest
    /// </summary>
    [Required]
    public string Label { get; set; } = null!;

    /// <summary>
    /// Problem name
    /// </summary>
    [Required]
    public string Name { get; set; } = null!;
}
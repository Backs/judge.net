using System.ComponentModel.DataAnnotations;

namespace Judge.Web.Client.Contests;

/// <summary>
/// Contest task
/// </summary>
public class ContestTask
{
    /// <summary>
    /// Unique task label
    /// </summary>
    [Required]
    public string Label { get; set; } = null!;
    
    /// <summary>
    /// Task name
    /// </summary>
    [Required]
    public string Name { get; set; } = null!;
    
    /// <summary>
    /// Task solved by current user
    /// </summary>
    [Required]
    public bool Solved { get; set; }
}
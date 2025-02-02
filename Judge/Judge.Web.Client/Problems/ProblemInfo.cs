using System.ComponentModel.DataAnnotations;

namespace Judge.Web.Client.Problems;

/// <summary>
/// Problem info
/// </summary>
public sealed class ProblemInfo
{
    /// <summary>
    /// Problem id
    /// </summary>
    [Required]
    public long Id { get; set; }

    /// <summary>
    /// Problem name
    /// </summary>
    [Required]
    public string Name { get; set; } = null!;

    /// <summary>
    /// Is problem solve by current user
    /// </summary>
    [Required]
    public bool Solved { get; set; }
}
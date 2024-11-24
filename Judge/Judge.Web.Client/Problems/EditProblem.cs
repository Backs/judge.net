using System.ComponentModel.DataAnnotations;

namespace Judge.Web.Client.Problems;

/// <summary>
/// Creates or edit problem
/// </summary>
public class EditProblem
{
    /// <summary>
    /// Problem id
    /// </summary>
    /// <remarks>Creates new if empty</remarks>
    public long? Id { get; set; }
    
    /// <summary>
    /// Problem name
    /// </summary>
    [Required]
    public string Name { get; set; } = null!;
    
    /// <summary>
    /// Memory limits in bytes
    /// </summary>
    [Required]
    public int MemoryLimitBytes { get; set; }
    
    /// <summary>
    /// Time limits in milliseconds
    /// </summary>
    [Required]
    public int TimeLimitMilliseconds { get; set; }
    
    /// <summary>
    /// Problem statement in markdown
    /// </summary>
    [Required]
    public string Statement { get; set; } = null!;
    
    /// <summary>
    /// Folder path with tests
    /// </summary>
    [Required]
    public string TestsFolder { get; set; } = null!;
    
    /// <summary>
    /// Is problem opened in archive
    /// </summary>
    [Required]
    public bool IsOpened { get; set; }
}
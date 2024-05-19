using System.ComponentModel.DataAnnotations;

namespace Judge.Web.Client.Problems;

public class Problem
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
    /// Memory limit in bytes
    /// </summary>
    [Required]
    public long MemoryLimitBytes { get; set; }
    
    /// <summary>
    /// Time limit in milliseconds
    /// </summary>
    [Required]
    public long TimeLimitMilliseconds { get; set; }
    
    /// <summary>
    /// Problem statement in markdown
    /// </summary>
    [Required]
    public string Statement { get; set; } = null!;
}
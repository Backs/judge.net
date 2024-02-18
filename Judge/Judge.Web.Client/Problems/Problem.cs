namespace Judge.Web.Client.Problems;

public class Problem
{
    /// <summary>
    /// Problem id
    /// </summary>
    public long Id { get; set; }
    
    /// <summary>
    /// Problem name
    /// </summary>
    public string Name { get; set; } = null!;
    
    /// <summary>
    /// Memory limit in bytes
    /// </summary>
    public long MemoryLimitBytes { get; set; }
    
    /// <summary>
    /// Time limit in milliseconds
    /// </summary>
    public long TimeLimitMilliseconds { get; set; }
    
    /// <summary>
    /// Problem statement in markdown
    /// </summary>
    public string Statement { get; set; } = null!;
}
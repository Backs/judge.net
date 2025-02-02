using System.ComponentModel.DataAnnotations;

namespace Judge.Web.Client.Problems;

/// <summary>
/// Problems query
/// </summary>
public class ProblemsQuery
{
    /// <summary>
    /// Problem name
    /// </summary>
    [MinLength(2)]
    public string? Name { get; set; }
    
    /// <summary>
    /// Problems to skip
    /// </summary>
    [Range(0, int.MaxValue)]
    public int Skip { get; set; }

    /// <summary>
    /// Problems to take
    /// </summary>
    [Range(1, 100)]
    public int Take { get; set; } = 20;
}
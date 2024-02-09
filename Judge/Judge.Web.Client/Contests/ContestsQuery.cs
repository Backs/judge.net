using System.ComponentModel.DataAnnotations;

namespace Judge.Web.Client.Contests;

/// <summary>
/// Contests query
/// </summary>
public class ContestsQuery
{
    /// <summary>
    /// Contests to skip
    /// </summary>
    [Range(0, int.MaxValue)]
    public int Skip { get; set; } = 0;

    /// <summary>
    /// Max items in result
    /// </summary>
    [Range(1, 100)]
    public int Take { get; set; } = 50;
}
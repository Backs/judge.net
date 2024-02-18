using System.ComponentModel.DataAnnotations;

namespace Judge.Web.Client.Submits;

public class SubmitsQuery
{
    /// <summary>
    /// Problem id
    /// </summary>
    public long? ProblemId { get; set; }

    /// <summary>
    /// Contest id
    /// </summary>
    public int? ContestId { get; set; }

    /// <summary>
    /// Problem label
    /// </summary>
    public string? ProblemLabel { get; set; }
    
    /// <summary>
    /// Submits to skip
    /// </summary>
    [Range(0, int.MaxValue)]
    public int Skip { get; set; }

    /// <summary>
    /// Submits to take
    /// </summary>
    [Range(1, 100)]
    public int Take { get; set; } = 20;
}
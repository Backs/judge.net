using System.ComponentModel.DataAnnotations;

namespace Judge.Web.Client.Submits;

public class SubmitsQuery
{
    public long? ProblemId { get; set; }

    public int? ContestId { get; set; }

    public string? TaskLabel { get; set; }
    
    [Range(0, int.MaxValue)]
    public int Skip { get; set; }

    [Range(1, 100)]
    public int Take { get; set; } = 20;
}
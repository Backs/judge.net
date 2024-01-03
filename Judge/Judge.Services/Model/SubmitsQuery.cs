namespace Judge.Services.Model;

public class SubmitsQuery
{
    public SubmitsType Type { get; set; } = SubmitsType.All;
    public long? ProblemId { get; set; }

    public int? ContestId { get; set; }

    public string? TaskLabel { get; set; }

    public long? UserId { get; set; }
    
    public int Skip { get; set; }

    public int Take { get; set; } = 20;
}
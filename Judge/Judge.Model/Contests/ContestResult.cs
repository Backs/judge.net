namespace Judge.Model.Contests;

public sealed class ContestResult
{
    public long UserId { get; set; }

    public ContestTaskResult[] TaskResults { get; set; }
}
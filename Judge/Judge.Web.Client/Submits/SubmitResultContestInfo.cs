namespace Judge.Web.Client.Submits;

public sealed class SubmitResultContestInfo
{
    /// <summary>
    /// Contest id
    /// </summary>
    public int ContestId { get; set; }
    
    /// <summary>
    /// Label of problem in contest
    /// </summary>
    public string Label { get; set; } = null!;
}
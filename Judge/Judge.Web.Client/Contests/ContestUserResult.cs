using System.Collections.Generic;

namespace Judge.Web.Client.Contests;

public class ContestUserResult
{
    public int Position { get; set; }
    public string UserName { get; set; } = null!;
    public long UserId { get; set; }
    public Dictionary<string, ContestTaskResult> Tasks { get; set; } = new();
    public int SolvedCount { get; set; }
    public int Points { get; set; }
}
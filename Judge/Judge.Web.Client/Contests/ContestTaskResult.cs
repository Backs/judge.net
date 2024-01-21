using System;

namespace Judge.Web.Client.Contests;

public class ContestTaskResult
{
    public int Points { get; set; }
    public int Attempts { get; set; }
    public TimeSpan? Time { get; set; }
    public bool Solved { get; set; }
}
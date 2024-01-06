using System;

namespace Judge.Web.Client.Contests;

public class Contest
{
    public string Name { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public TimeSpan Duration { get; set; }
    public ContestStatus Status { get; set; }
}
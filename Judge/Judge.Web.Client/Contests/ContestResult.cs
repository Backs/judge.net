using System;

namespace Judge.Web.Client.Contests;

public class ContestResult : Contest
{
    public ContestRules Rules { get; set; }

    public ContestUserResult[] Users { get; set; } = Array.Empty<ContestUserResult>();
}
using System;

namespace Judge.Web.Client.Contests;

public class Contest : ContestInfo
{
    public ContestTask[] Tasks { get; set; } = Array.Empty<ContestTask>();
}
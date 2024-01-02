﻿namespace Judge.Web.Client.Problems;

public sealed class ProblemInfo
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public bool Solved { get; set; }
}
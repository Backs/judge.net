using System;

namespace Judge.Web.Client.Submits;

public class SubmitsList
{
    public SubmitResultInfo[] Items { get; set; } = Array.Empty<SubmitResultInfo>();
    public int TotalCount { get; set; }
}
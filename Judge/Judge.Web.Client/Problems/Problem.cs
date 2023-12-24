namespace Judge.Web.Client.Problems;

public class Problem
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public long MemoryLimitBytes { get; set; }
    public long TimeLimitMilliseconds { get; set; }
    public string Statement { get; set; } = null!;
}
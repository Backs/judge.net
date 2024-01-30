namespace Judge.Web.Client.Problems;

public class EditProblem
{
    public long? Id { get; set; }
    public string Name { get; set; } = null!;
    public int MemoryLimitBytes { get; set; }
    public int TimeLimitMilliseconds { get; set; }
    public string Statement { get; set; } = null!;
    public string TestsFolder { get; set; } = null!;
    public bool IdOpened { get; set; }
}
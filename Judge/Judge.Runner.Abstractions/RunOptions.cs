namespace Judge.Runner.Abstractions;

public class RunOptions
{
    public string WorkingDirectory { get; set; }
    public string Executable { get; set; }
    public string Input { get; set; }
    public string Output { get; set; }
    public string Arguments { get; set; }
    public TimeSpan TimeLimit { get; set; }
    public int MemoryLimitBytes { get; set; }
}
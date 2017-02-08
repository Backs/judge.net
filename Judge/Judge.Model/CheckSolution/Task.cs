namespace Judge.Model.CheckSolution
{
    public sealed class Task
    {
        public long Id { get; set; }
        public string TestsFolder { get; set; }
        public int TimeLimitMilliseconds { get; set; }
        public int MemoryLimitBytes { get; set; }
    }
}

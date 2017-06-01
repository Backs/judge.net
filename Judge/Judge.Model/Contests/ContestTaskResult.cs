namespace Judge.Model.Contests
{
    public sealed class ContestTaskResult
    {
        public long ProblemId { get; set; }
        public int Attempts { get; set; }
        public bool Solved { get; set; }
    }
}

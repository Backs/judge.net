using Judge.Model.CheckSolution;

namespace Judge.Model.Contests
{
    public sealed class ContestTask
    {
        public Contest Contest { get; set; }
        public string TaskName { get; set; }
        public Task Task { get; set; }
        public long TaskId { get; set; }
    }
}
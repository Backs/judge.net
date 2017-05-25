using Judge.Model.CheckSolution;

namespace Judge.Model.Contests
{
    public sealed class ContestTask
    {
        public int ContestId { get; set; }
        public string TaskName { get; set; }
        public Task Task { get; set; }
    }
}
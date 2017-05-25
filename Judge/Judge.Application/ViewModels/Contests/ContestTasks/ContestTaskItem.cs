namespace Judge.Application.ViewModels.Contests.ContestTasks
{
    public class ContestTaskItem
    {
        public string Label { get; set; }
        public string Name { get; set; }
        public bool Solved { get; set; }
        public long ProblemId { get; set; }
    }
}
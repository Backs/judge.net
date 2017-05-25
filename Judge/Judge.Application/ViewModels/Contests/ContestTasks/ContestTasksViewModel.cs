using System.Collections.Generic;

namespace Judge.Application.ViewModels.Contests.ContestTasks
{
    public sealed class ContestTasksViewModel
    {
        public ContestTasksViewModel(IEnumerable<ContestTaskItem> tasks)
        {
            Tasks = tasks;
        }

        public string ContestName { get; set; }
        public long ContestId { get; set; }

        public IEnumerable<ContestTaskItem> Tasks { get; }
    }
}
using System.Collections.Generic;
using Judge.Application.ViewModels.Contests.ContestsList;

namespace Judge.Application.ViewModels.Contests.ContestTasks
{
    public sealed class ContestTasksViewModel
    {
        public ContestTasksViewModel(IEnumerable<ContestTaskItem> tasks)
        {
            Tasks = tasks;
        }

        public ContestItem Contest { get; set; }

        public IEnumerable<ContestTaskItem> Tasks { get; }
    }
}
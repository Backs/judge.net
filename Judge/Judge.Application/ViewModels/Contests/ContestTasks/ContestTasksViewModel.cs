using System.Collections.Generic;
using Judge.Application.ViewModels.Contests.ContestsList;

namespace Judge.Application.ViewModels.Contests.ContestTasks
{
    public sealed class ContestTasksViewModel
    {
        public ContestTasksViewModel(ICollection<ContestTaskItem> tasks)
        {
            Tasks = tasks;
        }

        public ContestItem Contest { get; set; }

        public ICollection<ContestTaskItem> Tasks { get; }
    }
}
using System.Collections.Generic;

namespace Judge.Application.ViewModels.Contests.ContestResult
{
    public class ContestResultViewModel
    {
        public int ContestId { get; set; }
        public IEnumerable<TaskViewModel> Tasks { get; set; }
        public IEnumerable<ContestUserResultViewModel> Users { get; set; }
    }
}
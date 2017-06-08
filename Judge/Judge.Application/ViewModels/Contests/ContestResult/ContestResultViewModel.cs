using System.Collections.Generic;
using Judge.Application.ViewModels.Contests.ContestsList;

namespace Judge.Application.ViewModels.Contests.ContestResult
{
    public class ContestResultViewModel
    {
        public ContestItem Contest { get; set; }
        public IEnumerable<TaskViewModel> Tasks { get; set; }
        public IEnumerable<ContestUserResultViewModel> Users { get; set; }
    }
}
using System.Collections.Generic;

namespace Judge.Application.ViewModels.Problems.ProblemsList
{
    public sealed class ProblemsListViewModel
    {
        public ProblemsListViewModel(ICollection<ProblemItem> problems)
        {
            Problems = problems;
        }

        public int ProblemsCount { get; set; }
        public PaginationViewModel Pagination { get; set; }

        public ICollection<ProblemItem> Problems { get; }
    }
}

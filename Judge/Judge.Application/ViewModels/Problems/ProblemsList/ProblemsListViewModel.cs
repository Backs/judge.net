using System.Collections.Generic;

namespace Judge.Application.ViewModels.Problems.ProblemsList
{
    public sealed class ProblemsListViewModel
    {
        public ProblemsListViewModel(IEnumerable<ProblemItem> problems)
        {
            Problems = problems;
        }

        public int ProblemsCount { get; set; }
        public PaginationViewModel Pagination { get; set; }

        public IEnumerable<ProblemItem> Problems { get; }
    }
}

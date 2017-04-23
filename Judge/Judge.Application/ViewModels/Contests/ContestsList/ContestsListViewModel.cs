using System.Collections.Generic;

namespace Judge.Application.ViewModels.Contests.ContestsList
{
    public sealed class ContestsListViewModel
    {
        public ContestsListViewModel(IEnumerable<ContestItem> contests)
        {
            Contests = contests;
        }

        public IEnumerable<ContestItem> Contests { get; }
    }
}

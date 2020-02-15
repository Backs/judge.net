using System.Collections.Generic;

namespace Judge.Application.ViewModels.Contests.ContestsList
{
    public sealed class ContestsListViewModel
    {
        public ContestsListViewModel(ICollection<ContestItem> contests)
        {
            Contests = contests;
        }

        public ICollection<ContestItem> Contests { get; }
    }
}

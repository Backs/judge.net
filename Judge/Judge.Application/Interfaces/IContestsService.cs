using Judge.Application.ViewModels.Contests.ContestsList;

namespace Judge.Application.Interfaces
{
    public interface IContestsService
    {
        ContestsListViewModel GetContests();
    }
}

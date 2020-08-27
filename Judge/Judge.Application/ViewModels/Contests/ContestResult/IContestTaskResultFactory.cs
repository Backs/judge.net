using Judge.Model.Contests;

namespace Judge.Application.ViewModels.Contests.ContestResult
{
    internal interface IContestTaskResultFactory
    {
        ContestTaskResultViewModelBase Convert(Contest contest, ContestTaskResult taskResult);
    }
}

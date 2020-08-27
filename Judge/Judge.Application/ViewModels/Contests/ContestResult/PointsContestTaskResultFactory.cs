using Judge.Model.Contests;

namespace Judge.Application.ViewModels.Contests.ContestResult
{
    internal sealed class PointsContestTaskResultFactory : IContestTaskResultFactory
    {
        public ContestTaskResultViewModelBase Convert(Contest contest, ContestTaskResult taskResult)
        {
            return new PointsContestTaskResultViewModel(taskResult.SubmitDateUtc)
            {
                Attempts = taskResult.Attempts,
                ProblemId = taskResult.ProblemId,
                Solved = taskResult.Solved
            };
        }
    }
}
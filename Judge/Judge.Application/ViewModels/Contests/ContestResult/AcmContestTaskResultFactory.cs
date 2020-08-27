using Judge.Model.Contests;

namespace Judge.Application.ViewModels.Contests.ContestResult
{
    internal sealed class AcmContestTaskResultFactory : IContestTaskResultFactory
    {
        public ContestTaskResultViewModelBase Convert(Contest contest, ContestTaskResult taskResult)
        {
            return new AcmContestTaskResultViewModel(contest.StartTime, taskResult.SubmitDateUtc)
            {
                Solved = taskResult.Solved,
                ProblemId = taskResult.ProblemId,
                Attempts = taskResult.Attempts
            };
        }
    }
}
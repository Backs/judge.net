namespace Judge.Application.ViewModels.Contests.ContestResult
{
    using System.Collections.Generic;
    using System.Linq;
    using Judge.Application.ViewModels.Contests.ContestsList;
    using Judge.Model.Contests;

    internal class CheckPointContestTaskResultFactory : IContestTaskResultFactory
    {
        public ContestResultViewModel Convert(IEnumerable<ContestTask> tasks, IEnumerable<ContestResult> results, IDictionary<long, string> users, Contest contest)
        {
            var userModels = results.Select(o => new CheckPointContestUserResultViewModel
            {
                UserName = users[o.UserId],
                Tasks = o.TaskResults.Select(t => Convert(contest, t)).ToDictionary(t => t.ProblemId)
            });

            return new ContestResultViewModel(userModels)
            {
                Contest = new ContestItem(contest),
                Tasks = tasks.Select(o => new TaskViewModel { Label = o.TaskName, ProblemId = o.Task.Id }).ToArray()
            };
        }

        private static ContestTaskResultViewModelBase Convert(Contest contest, ContestTaskResult taskResult)
        {
            return new CheckPointContestTaskResultViewModel(taskResult.SubmitDateUtc, contest.CheckPointTime.Value)
            {
                Solved = taskResult.Solved,
                ProblemId = taskResult.ProblemId,
                Attempts = taskResult.Attempts
            };
        }
    }
}

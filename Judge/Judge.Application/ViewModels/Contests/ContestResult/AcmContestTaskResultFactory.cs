using System.Collections.Generic;
using System.Linq;
using Judge.Application.ViewModels.Contests.ContestsList;
using Judge.Model.Contests;

namespace Judge.Application.ViewModels.Contests.ContestResult
{
    internal sealed class AcmContestTaskResultFactory : IContestTaskResultFactory
    {
        private static ContestTaskResultViewModelBase Convert(Contest contest, ContestTaskResult taskResult)
        {
            return new AcmContestTaskResultViewModel(contest.StartTime, taskResult.SubmitDateUtc)
            {
                Solved = taskResult.Solved,
                ProblemId = taskResult.ProblemId,
                Attempts = taskResult.Attempts
            };
        }

        public ContestResultViewModel Convert(
            IEnumerable<ContestTask> tasks,
            IEnumerable<Model.Contests.ContestResult> results,
            IDictionary<long, string> users,
            Contest contest)
        {
            var userModels = results.Select(o => new AcmContestUserResultViewModel
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
    }
}
using Judge.Application.ViewModels.Contests.ContestsList;
using Judge.Model.Contests;
using System.Collections.Generic;
using System.Linq;

namespace Judge.Application.ViewModels.Contests.ContestResult
{
    internal sealed class PointsContestTaskResultFactory : IContestTaskResultFactory
    {
        private static ContestTaskResultViewModelBase Convert(ContestTaskResult taskResult)
        {
            return new PointsContestTaskResultViewModel(taskResult.SubmitDateUtc)
            {
                Attempts = taskResult.Attempts,
                ProblemId = taskResult.ProblemId,
                Solved = taskResult.Solved
            };
        }

        public ContestResultViewModel Convert(
            IEnumerable<ContestTask> tasks,
            IEnumerable<Model.Contests.ContestResult> results,
            IDictionary<long, string> users,
            Contest contest)
        {
            var userModels = results.Select(o => new PointContestUserResultViewModel
            {
                UserName = users[o.UserId],
                Tasks = o.TaskResults.Select(Convert).ToDictionary(t => t.ProblemId)
            });

            return new ContestResultViewModel(userModels)
            {
                Contest = new ContestItem(contest),
                Tasks = tasks.Select(o => new TaskViewModel { Label = o.TaskName, ProblemId = o.Task.Id }).ToArray()
            };
        }
    }
}
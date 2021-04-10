namespace Judge.Application.ViewModels.Contests.ContestResult
{
    using System.Collections.Generic;
    using System.Linq;
    using Judge.Application.ViewModels.Contests.ContestsList;
    using Judge.Model.Contests;

    internal sealed class CheckPointContestTaskResultFactory : IContestTaskResultFactory
    {
        public ContestResultViewModel Convert(IEnumerable<ContestTask> tasks, IReadOnlyList<ContestResult> results,
        IDictionary<long, string> users, Contest contest)
        {
            var firstSolved = results.SelectMany(o => o.TaskResults, (a, b) => new { a.UserId, b.ProblemId, b.Solved, b.SubmitDateUtc })
                                .Where(o => o.Solved)
                                .GroupBy(o => o.ProblemId)
                                .Select(o => o.OrderBy(t => t.SubmitDateUtc).First())
                                .Select(o => (o.ProblemId, o.UserId))
                                .ToHashSet();

            var userModels = results.Select(o => new CheckPointContestUserResultViewModel
            {
                UserName = users[o.UserId],
                Tasks = o.TaskResults.Select(t => Convert(contest, t, o.UserId, firstSolved)).ToDictionary(t => t.ProblemId)
            });

            return new ContestResultViewModel(userModels)
            {
                Contest = new ContestItem(contest),
                Tasks = tasks.Select(o => new TaskViewModel { Label = o.TaskName, ProblemId = o.Task.Id }).ToArray()
            };
        }

        private static ContestTaskResultViewModelBase Convert(Contest contest, ContestTaskResult taskResult,
        long userId,
        ICollection<(long ProblemId, long UserId)> firstSolved)
        {
            return new CheckPointContestTaskResultViewModel(taskResult.SubmitDateUtc, contest.CheckPointTime.Value)
            {
                Solved = taskResult.Solved,
                ProblemId = taskResult.ProblemId,
                Attempts = taskResult.Attempts,
                FirstSolved = firstSolved.Contains((taskResult.ProblemId, userId))
            };
        }
    }
}

namespace Judge.Data.Repository
{
    using System.Collections.Generic;
    using System.Linq;
    using Judge.Model.Contests;
    using Judge.Model.SubmitSolution;

    internal sealed class ContestResultRepository : IContestResultRepository
    {
        private readonly DataContext context;

        public ContestResultRepository(DataContext context)
        {
            this.context = context;
        }

        public IEnumerable<ContestResult> Get(long contestId)
        {
            var contestTasks = this.context.Set<ContestTask>().Where(o => o.ContestId == contestId).Select(o => o.TaskId)
                .ToArray();

            var result = this.context.Set<ContestTaskSubmit>()
                .Where(o => o.ContestId == contestId && contestTasks.Contains(o.ProblemId))
                .Select(o => new
                {
                    o.UserId,
                    o.ProblemId,
                    Results = o.Results.Select(s => new
                    {
                        s.Id,
                        s.Status,
                        s.Submit.SubmitDateUtc
                    })
                })
                .ToArray()
                .GroupBy(o => o.UserId)
                .Select(o => new
                {
                    UserId = o.Key,
                    TaskResults = o.GroupBy(p => p.ProblemId).Select(p => new
                    {
                        ProblemId = p.Key,
                        SubmitResults = p.Select(s => s.Results.OrderByDescending(t => t.Id).FirstOrDefault()) //only last judge result
                                         .Where(s => s != null && s.Status != SubmitStatus.ServerError && s.Status != SubmitStatus.Pending && s.Status != SubmitStatus.CompilationError)
                                         .Select(s => new
                                         {
                                             s.Id,
                                             s.Status,
                                             s.SubmitDateUtc
                                         }).OrderBy(s => s.Id).ToArray()
                    })
                })
                .Select(o => new
                {
                    UserId = o.UserId,
                    TaskResults = o.TaskResults.Select(t => new
                    {
                        Solved = t.SubmitResults.Any(s => s.Status == SubmitStatus.Accepted),
                        ProblemId = t.ProblemId,
                        SubmitResults = t.SubmitResults,
                        FirstSuccess = t.SubmitResults.FirstOrDefault(s => s.Status == SubmitStatus.Accepted)
                    })
                })
                .Select(o => new ContestResult
                {
                    UserId = o.UserId,
                    TaskResults = o.TaskResults.Where(t => t.SubmitResults.Any()).Select(t => new ContestTaskResult
                    {
                        Solved = t.Solved,
                        ProblemId = t.ProblemId,
                        Attempts = t.FirstSuccess == null ? t.SubmitResults.Length : t.SubmitResults.Count(s => s.Id <= t.FirstSuccess.Id),
                        SubmitDateUtc = t.FirstSuccess?.SubmitDateUtc ?? t.SubmitResults.Last().SubmitDateUtc
                    })
                })
                .Where(t => t.TaskResults.Any());
            return result.AsEnumerable();
        }
    }
}

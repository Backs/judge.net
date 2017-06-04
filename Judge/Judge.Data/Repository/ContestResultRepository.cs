using System.Collections.Generic;
using System.Linq;
using Judge.Model.Contests;
using Judge.Model.SubmitSolution;

namespace Judge.Data.Repository
{
    internal sealed class ContestResultRepository : IContestResultRepository
    {
        private readonly DataContext _context;

        public ContestResultRepository(DataContext context)
        {
            _context = context;
        }

        public IEnumerable<ContestResult> Get(long contestId)
        {
            var result = _context.Set<ContestTaskSubmit>()
                .Where(o => o.ContestId == contestId)
                .GroupBy(o => o.UserId)
                .Select(o => new
                {
                    UserId = o.Key,
                    TaskResults = o.GroupBy(p => p.ProblemId).Select(p => new
                    {
                        ProblemId = p.Key,
                        SubmitResults = p.Select(s => s.Results.OrderByDescending(t => t.Id).FirstOrDefault()) //only last judge result
                                        .Select(s => new
                                        {
                                            s.Status,
                                            s.Submit.SubmitDateUtc,
                                            s.Id
                                        }).OrderBy(s => s.Id)
                    })
                })
                .AsEnumerable()
                .Select(o => new
                {
                    UserId = o.UserId,
                    TaskResults = o.TaskResults.Select(t => new
                    {
                        Solved = t.SubmitResults.Any(s => s.Status == SubmitStatus.Accepted),
                        ProblemId = t.ProblemId,
                        SubmitResults = t.SubmitResults.Where(s => s.Status != SubmitStatus.ServerError && s.Status != SubmitStatus.Pending && s.Status != SubmitStatus.CompilationError),
                        FirstSuccess = t.SubmitResults.FirstOrDefault(s => s.Status == SubmitStatus.Accepted)
                    })
                })
                .Select(o => new ContestResult
                {
                    UserId = o.UserId,
                    TaskResults = o.TaskResults.Select(t => new ContestTaskResult
                    {
                        Solved = t.Solved,
                        ProblemId = t.ProblemId,
                        Attempts = t.FirstSuccess == null ? t.SubmitResults.Count() : t.SubmitResults.Count(s => s.Id <= t.FirstSuccess.Id),
                        SubmitDateUtc = t.FirstSuccess == null ? t.SubmitResults.Last().SubmitDateUtc : t.FirstSuccess.SubmitDateUtc
                    })
                });
            return result.AsEnumerable();
        }
    }
}

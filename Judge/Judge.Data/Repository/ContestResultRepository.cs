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
            var result = _context.Set<SubmitResult>()
                .Where(o => o.Submit is ContestTaskSubmit)
                .Where(o => (o.Submit as ContestTaskSubmit).ContestId == contestId)
                .GroupBy(o => o.Submit.UserId)
                .Select(o => new
                {
                    UserId = o.Key,
                    Tasks = o.GroupBy(p => p.Submit.ProblemId).Select(p => new
                    {
                        ProblemId = p.Key,
                        Solved = p.Any(t => t.Status == SubmitStatus.Accepted)
                    })
                })
                .Select(o => new ContestResult
                {
                    UserId = o.UserId,
                    TaskResults = o.Tasks.Select(t => new ContestTaskResult
                    {
                        Solved = t.Solved,
                        ProblemId = t.ProblemId
                    })
                });

            return result.AsEnumerable();
        }
    }
}

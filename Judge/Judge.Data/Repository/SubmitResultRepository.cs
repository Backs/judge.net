using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Judge.Model.SubmitSolution;

namespace Judge.Data.Repository
{
    internal sealed class SubmitResultRepository : ISubmitResultRepository
    {
        private readonly DataContext _context;

        public SubmitResultRepository(DataContext context)
        {
            _context = context;
        }

        public IEnumerable<SubmitResult> GetLastSubmits(long? userId, long? problemId, int count)
        {
            var query = _context.Set<SubmitResult>() as IQueryable<SubmitResult>;
            if (userId != null)
            {
                query = query.Where(o => o.Submit.UserId == userId);
            }
            if (problemId != null)
            {
                query = query.Where(o => o.Submit.ProblemId == problemId);
            }
            query = query.OrderByDescending(o => o.Id);

            query = query.Take(count);

            return query.Include(o => o.Submit).AsEnumerable();
        }

        //TODO: domain service method
        public IEnumerable<long> GetSolvedProblems(IEnumerable<long> problems)
        {
            return _context.Set<SubmitResult>()
                .Where(o => problems.Contains(o.Submit.ProblemId))
                .Where(o => o.Status == SubmitStatus.Accepted)
                .Select(o => o.Submit.ProblemId)
                .Distinct()
                .AsEnumerable();
        }

        public SubmitResult DequeueUnchecked()
        {
            var check = _context.DequeueSubmitCheck();

            if (check == null) return null;

            return _context.Set<SubmitResult>().Where(o => o.Id == check.SubmitResultId)
                .Include(o => o.Submit).First();
        }
    }
}

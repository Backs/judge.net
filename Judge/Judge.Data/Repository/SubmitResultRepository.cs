using System;
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

        public IEnumerable<SubmitResult> GetSubmits(long? userId, long? problemId, int page, int pageSize)
        {
            if (page <= 0)
                throw new ArgumentOutOfRangeException(nameof(page));
            if (pageSize <= 0)
                throw new ArgumentOutOfRangeException(nameof(pageSize));

            var query = _context.Set<SubmitResult>() as IQueryable<SubmitResult>;

            query = query.Where(o => o.Submit is ProblemSubmit);

            if (userId != null)
            {
                query = query.Where(o => o.Submit.UserId == userId);
            }
            if (problemId != null)
            {
                query = query.Where(o => o.Submit.ProblemId == problemId);
            }
            query = query.OrderByDescending(o => o.Id);

            var skip = (page - 1) * pageSize;

            if (skip > 0)
            {
                query = query.Skip(skip);
            }

            query = query.Take(pageSize);

            return query.Include(o => o.Submit).AsEnumerable();
        }

        public IEnumerable<long> GetSolvedProblems(long userId, IEnumerable<long> problems)
        {
            return _context.Set<SubmitResult>()
                .Where(o => o.Submit.UserId == userId && problems.Contains((o.Submit as ProblemSubmit).ProblemId))
                .Where(o => o.Status == SubmitStatus.Accepted)
                .Select(o => (o.Submit as ProblemSubmit).ProblemId)
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

        public int Count(long? problemId, long? userId)
        {
            var query = _context.Set<SubmitResult>() as IQueryable<SubmitResult>;

            query = query.Where(o => o.Submit is ProblemSubmit);

            if (problemId != null)
            {
                query = query.Where(o => o.Submit.ProblemId == problemId);
            }
            if (userId != null)
            {
                query = query.Where(o => o.Submit.UserId == userId);
            }
            return query.Count();
        }
    }
}

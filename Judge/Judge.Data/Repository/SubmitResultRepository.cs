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

        public IEnumerable<SubmitResult> GetLastSubmits(long? userId, int count)
        {
            var query = _context.Set<SubmitResult>() as IQueryable<SubmitResult>;
            if (userId != null)
            {
                query = query.Where(o => o.Submit.UserId == userId);
            }
            query = query.OrderByDescending(o => o.Id);

            query = query.Take(count);

            return query.Include(o => o.Submit).AsEnumerable();
        }
    }
}

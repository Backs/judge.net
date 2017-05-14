using System.Linq;
using Judge.Model.SubmitSolution;

namespace Judge.Data.Repository
{
    internal sealed class SubmitRepository : ISubmitRepository
    {
        private readonly DataContext _context;

        public SubmitRepository(DataContext context)
        {
            _context = context;
        }

        public void Add(ProblemSubmit item)
        {
            _context.Set<ProblemSubmit>().Add(item);
        }

        public ProblemSubmit Get(long submitId)
        {
            return _context.Set<ProblemSubmit>().AsNoTracking().FirstOrDefault(o => o.Id == submitId);
        }
    }
}

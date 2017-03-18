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

        public void Add(Submit item)
        {
            _context.Set<Submit>().Add(item);
        }

        public Submit Get(long submitId)
        {
            return _context.Set<Submit>().AsNoTracking().FirstOrDefault(o => o.Id == submitId);
        }
    }
}

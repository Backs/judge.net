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

        public void Add(SubmitBase item)
        {
            _context.Set<SubmitBase>().Add(item);
        }

        public SubmitBase Get(long submitId)
        {
            return _context.Set<SubmitBase>().AsNoTracking().FirstOrDefault(o => o.Id == submitId);
        }
    }
}

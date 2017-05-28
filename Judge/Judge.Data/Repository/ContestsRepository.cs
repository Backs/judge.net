using System.Collections.Generic;
using System.Linq;
using Judge.Model.Contests;

namespace Judge.Data.Repository
{
    internal sealed class ContestsRepository : IContestsRepository
    {
        private readonly DataContext _context;

        public ContestsRepository(DataContext context)
        {
            _context = context;
        }

        public IEnumerable<Contest> GetList()
        {
            return _context.Set<Contest>().OrderByDescending(o => o.StartTime).AsEnumerable();
        }

        public Contest Get(int id)
        {
            return _context.Set<Contest>().FirstOrDefault(o => o.Id == id);
        }
    }
}

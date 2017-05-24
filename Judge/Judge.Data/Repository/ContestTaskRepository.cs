using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Judge.Model.Contests;

namespace Judge.Data.Repository
{
    internal sealed class ContestTaskRepository : IContestTaskRepository
    {
        private readonly DataContext _context;

        public ContestTaskRepository(DataContext context)
        {
            _context = context;
        }

        public IEnumerable<ContestTask> GetTasks(long contestId)
        {
            return _context.Set<ContestTask>()
                            .Where(o => o.ContestId == contestId)
                            .Include(o => o.Task)
                            .OrderBy(o => o.TaskName)
                            .AsEnumerable();
        }
    }
}

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

        public void Add(ContestTask task)
        {
            _context.Set<ContestTask>().Add(task);
        }

        public void Delete(ContestTask task)
        {
            _context.Set<ContestTask>().Remove(task);
        }

        public IEnumerable<ContestTask> GetTasks(int contestId)
        {
            return _context.Set<ContestTask>()
                            .Where(o => o.Contest.Id == contestId)
                            .Include(o => o.Task)
                            .Include(o => o.Contest)
                            .OrderBy(o => o.TaskName)
                            .AsEnumerable();
        }

        public ContestTask Get(int contestId, string label)
        {
            return _context.Set<ContestTask>()
                    .Include(o => o.Task)
                    .Include(o => o.Contest)
                    .FirstOrDefault(o => o.Contest.Id == contestId && o.TaskName == label);
        }

        public IEnumerable<ContestTask> GetTasks()
        {
            return _context.Set<ContestTask>()
                .Include(o => o.Task)
                .OrderBy(o => o.TaskName)
                .AsEnumerable();
        }
    }
}

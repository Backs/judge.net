using System.Collections.Generic;
using System.Linq;
using Judge.Model.CheckSolution;
using Task = Judge.Model.CheckSolution.Task;

namespace Judge.Data.Repository
{
    internal sealed class TaskRepository : ITaskRepository
    {
        private readonly DataContext _context;

        public TaskRepository(DataContext context)
        {
            _context = context;
        }

        public Task Get(long problemId)
        {
            return _context.Set<Task>().FirstOrDefault(o => o.Id == problemId);
        }

        public void Add(Task problem)
        {
            _context.Set<Task>().Add(problem);
        }

        public IEnumerable<Task> GetTasks(IEnumerable<long> ids)
        {
            return _context.Set<Task>()
                .Where(o => ids.Contains(o.Id))
                .AsEnumerable();
        }
    }
}

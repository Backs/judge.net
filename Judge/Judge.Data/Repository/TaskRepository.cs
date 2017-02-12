using System;
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

        public IEnumerable<Task> GetTaskList(int page, int pageSize)
        {
            if (page <= 0)
                throw new ArgumentOutOfRangeException(nameof(page));
            if (pageSize <= 0)
                throw new ArgumentOutOfRangeException(nameof(pageSize));

            var skip = (page - 1) * pageSize;

            IQueryable<Task> taskList = _context.Set<Task>().OrderBy(o => o.Id);
            if (skip > 0)
            {
                taskList = taskList.Skip(skip);
            }
            taskList = taskList.Take(pageSize);
            return taskList.AsEnumerable();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Judge.Model;
using Judge.Model.CheckSolution;

namespace Judge.Data.Repository
{
    internal sealed class TaskNameRepository : ITaskNameRepository
    {
        private readonly DataContext _context;

        public TaskNameRepository(DataContext context)
        {
            _context = context;
        }

        public IEnumerable<TaskName> GetTasks(ISpecification<Task> specification, int page, int pageSize)
        {
            if (page <= 0)
                throw new ArgumentOutOfRangeException(nameof(page));
            if (pageSize <= 0)
                throw new ArgumentOutOfRangeException(nameof(pageSize));

            var skip = (page - 1) * pageSize;

            IQueryable<Task> taskList = _context.Set<Task>().Where(specification.IsSatisfiedBy).OrderBy(o => o.Id);
            if (skip > 0)
            {
                taskList = taskList.Skip(skip);
            }
            return taskList.Take(pageSize).Select(o => new TaskName { Id = o.Id, Name = o.Name, IsOpened = o.IsOpened }).AsEnumerable();
        }

        public IEnumerable<TaskName> GetTasks(IEnumerable<long> tasks)
        {
            return _context.Set<Task>()
                .Where(o => tasks.Contains(o.Id))
                .Select(o => new TaskName { Id = o.Id, Name = o.Name, IsOpened = o.IsOpened })
                .AsEnumerable();
        }

        public int Count()
        {
            return _context.Set<Task>().Count();
        }

        public TaskName Get(long id)
        {
            return _context.Set<Task>()
                .Where(o => o.Id == id)
                .Select(o => new TaskName { Id = o.Id, Name = o.Name, IsOpened = o.IsOpened })
                .FirstOrDefault();
        }
    }
}
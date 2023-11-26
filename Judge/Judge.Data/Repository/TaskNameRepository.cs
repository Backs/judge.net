using System;
using System.Collections.Generic;
using System.Linq;
using Judge.Model;
using Judge.Model.CheckSolution;

namespace Judge.Data.Repository
{
    internal sealed class TaskNameRepository : ITaskNameRepository
    {
        private readonly DataContext context;

        public TaskNameRepository(DataContext context)
        {
            this.context = context;
        }

        public IEnumerable<TaskName> GetTasks(ISpecification<Task> specification, int page, int pageSize)
        {
            if (page <= 0)
                throw new ArgumentOutOfRangeException(nameof(page));
            if (pageSize <= 0)
                throw new ArgumentOutOfRangeException(nameof(pageSize));

            var skip = (page - 1) * pageSize;

            IQueryable<Task> taskList = this.context.Set<Task>().Where(specification.IsSatisfiedBy).OrderBy(o => o.Id);
            if (skip > 0)
            {
                taskList = taskList.Skip(skip);
            }
            return taskList.Take(pageSize).Select(o => new TaskName { Id = o.Id, Name = o.Name, IsOpened = o.IsOpened }).AsEnumerable();
        }

        public IEnumerable<TaskName> GetTasks(IEnumerable<long> tasks)
        {
            return this.context.Set<Task>()
                .Where(o => tasks.Contains(o.Id))
                .Select(o => new TaskName { Id = o.Id, Name = o.Name, IsOpened = o.IsOpened })
                .AsEnumerable();
        }

        public int Count(bool openedOnly)
        {
            return openedOnly ?
                this.context.Set<Task>().Count(o => o.IsOpened == true) :
                this.context.Set<Task>().Count();
        }

        public TaskName Get(long id)
        {
            return this.context.Set<Task>()
                .Where(o => o.Id == id)
                .Select(o => new TaskName { Id = o.Id, Name = o.Name, IsOpened = o.IsOpened })
                .FirstOrDefault();
        }
    }
}
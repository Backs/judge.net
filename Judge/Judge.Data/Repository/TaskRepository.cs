using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Judge.Model;
using Judge.Model.CheckSolution;
using Microsoft.EntityFrameworkCore;
using Task = Judge.Model.CheckSolution.Task;

namespace Judge.Data.Repository;

internal sealed class TaskRepository : ITaskRepository
{
    private readonly DataContext context;

    public TaskRepository(DataContext context)
    {
        this.context = context;
    }

    public Task? Get(long problemId)
    {
        return this.context.Set<Task>().FirstOrDefault(o => o.Id == problemId);
    }

    public void Add(Task problem)
    {
        this.context.Set<Task>().Add(problem);
    }

    public IEnumerable<Task> GetTasks(IEnumerable<long> ids)
    {
        return this.context.Set<Task>()
            .Where(o => ids.Contains(o.Id))
            .AsEnumerable();
    }

    public async Task<IReadOnlyCollection<Task>> GetAsync(IEnumerable<long> ids)
    {
        return await this.context.Set<Task>()
            .Where(o => ids.Contains(o.Id))
            .ToListAsync();
    }

    public async Task<IReadOnlyCollection<Task>> GetTasksAsync(ISpecification<Task> specification, int skip, int take)
    {
        IQueryable<Task> taskList = this.context.Set<Task>().Where(specification.IsSatisfiedBy).OrderBy(o => o.Id);
        if (skip > 0)
        {
            taskList = taskList.Skip(skip);
        }

        return await taskList.Take(take).ToListAsync();
    }

    public Task<int> CountAsync(ISpecification<Task> specification)
    {
        return this.context.Set<Task>().Where(specification.IsSatisfiedBy).CountAsync();
    }

    public Task<Task> GetAsync(long id)
    {
        return this.context.Set<Task>().FirstOrDefaultAsync(o => o.Id == id);
    }
}
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Judge.Model.CheckSolution
{
    public interface ITaskRepository
    {
        Task Get(long problemId);
        void Add(Task problem);
        IEnumerable<Task> GetTasks(IEnumerable<long> ids);
        Task<IReadOnlyCollection<Task>> GetAsync(IEnumerable<long> ids);
        Task<IReadOnlyCollection<Task>> GetTasksAsync(ISpecification<Task> specification, int skip, int take);
        Task<int> CountAsync(ISpecification<Task> specification);
        Task<Task> GetAsync(long id);
    }
}
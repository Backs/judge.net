using System.Collections.Generic;
using System.Threading.Tasks;

namespace Judge.Model.CheckSolution
{
    public interface ITaskRepository
    {
        Task Get(long problemId);
        void Add(Task problem);
        IEnumerable<Task> GetTasks(IEnumerable<long> ids);
        Task<Task[]> GetTasksAsync(ISpecification<Task> specification, int skip, int take);
        Task<int> CountAsync(ISpecification<Task> specification);
    }
}
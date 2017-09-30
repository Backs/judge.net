using System.Collections.Generic;

namespace Judge.Model.CheckSolution
{
    public interface ITaskNameRepository
    {
        IEnumerable<TaskName> GetTasks(ISpecification<Task> specification, int page, int pageSize);
        IEnumerable<TaskName> GetTasks(IEnumerable<long> tasks);
        int Count();
    }
}
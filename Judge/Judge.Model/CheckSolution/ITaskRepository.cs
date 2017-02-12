using System.Collections.Generic;

namespace Judge.Model.CheckSolution
{
    public interface ITaskRepository
    {
        Task Get(long problemId);
        IEnumerable<Task> GetTaskList(int page, int pageSize);
    }
}

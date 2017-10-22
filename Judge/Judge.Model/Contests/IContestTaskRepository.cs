using System.Collections.Generic;

namespace Judge.Model.Contests
{
    public interface IContestTaskRepository
    {
        void Add(ContestTask task);
        void Delete(ContestTask task);
        IEnumerable<ContestTask> GetTasks(int contestId);
        ContestTask Get(int contestId, string label);
        IEnumerable<ContestTask> GetTasks();
    }
}
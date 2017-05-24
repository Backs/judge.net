using System.Collections.Generic;

namespace Judge.Model.Contests
{
    public interface IContestTaskRepository
    {
        IEnumerable<ContestTask> GetTasks(long contestId);
    }
}
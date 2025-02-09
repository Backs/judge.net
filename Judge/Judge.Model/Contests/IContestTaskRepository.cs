using System.Collections.Generic;
using System.Threading.Tasks;

namespace Judge.Model.Contests
{
    public interface IContestTaskRepository
    {
        void Add(ContestTask task);
        void Delete(ContestTask task);
        Task<IReadOnlyCollection<ContestTask>> SearchAsync(IEnumerable<int> contestIds);
        ContestTask Get(int contestId, string label);
        Task<ContestTask> TryGetAsync(int contestId, string label);
        Task<IReadOnlyCollection<ContestTask>> SearchAsync(int contestId);
        Task<IReadOnlyCollection<ContestResult>> GetResultsAsync(long contestId);
    }
}
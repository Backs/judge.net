using System.Linq;
using System.Collections.Generic;

namespace Judge.Application.ViewModels.Contests.ContestResult
{
    public sealed class ContestUserResultViewModel
    {
        public long UserId { get; set; }
        public IReadOnlyDictionary<long, ContestTaskResultViewModel> Tasks { get; set; }
        public string UserName { get; set; }
        public int SolvedCount => Tasks.Count(s => s.Value.Solved);
        public int Time => Tasks.Sum(o => o.Value.GetTime());

        public ContestTaskResultViewModel TryGetTask(long problemId)
        {
            return Tasks.TryGetValue(problemId, out var value) ? value : null;
        }
    }
}

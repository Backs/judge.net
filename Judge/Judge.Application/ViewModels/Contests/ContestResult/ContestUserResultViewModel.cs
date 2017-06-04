using System.Linq;
using System.Collections.Generic;

namespace Judge.Application.ViewModels.Contests.ContestResult
{
    public sealed class ContestUserResultViewModel
    {
        public long UserId { get; set; }
        public IReadOnlyCollection<ContestTaskResultViewModel> Tasks { get; set; }
        public string UserName { get; set; }
        public int SolvedCount => Tasks.Count(s => s.Solved);
        public int Time => Tasks.Sum(o => o.GetTime());
    }
}

using System;
using System.Collections.Generic;
using System.Linq;

namespace Judge.Application.ViewModels.Contests.ContestResult
{
    public abstract class ContestUserResultViewModelBase : IComparable
    {
        public IReadOnlyDictionary<long, ContestTaskResultViewModelBase> Tasks { get; set; }
        public string UserName { get; set; }
        public int SolvedCount => Tasks.Count(s => s.Value.Solved);
        public int Score => Tasks.Sum(o => o.Value.GetScore());
        public int Place { get; set; }

        public ContestTaskResultViewModelBase TryGetTask(long problemId)
        {
            return Tasks.TryGetValue(problemId, out var value) ? value : null;
        }

        public abstract int CompareTo(object other);
    }
}

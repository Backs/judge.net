using System;
using System.Linq;
using System.Collections.Generic;

namespace Judge.Application.ViewModels.Contests.ContestResult
{
    public sealed class ContestUserResultViewModel : IComparable<ContestUserResultViewModel>
    {
        public long UserId { get; set; }
        public IReadOnlyDictionary<long, ContestTaskResultViewModel> Tasks { get; set; }
        public string UserName { get; set; }
        public int SolvedCount => Tasks.Count(s => s.Value.Solved);
        public int Score => Tasks.Sum(o => o.Value.GetScore());
        public int Place { get; set; }

        public ContestTaskResultViewModel TryGetTask(long problemId)
        {
            return Tasks.TryGetValue(problemId, out var value) ? value : null;
        }

        public int CompareTo(ContestUserResultViewModel other)
        {
            if (SolvedCount == other.SolvedCount)
            {
                return Score.CompareTo(other.Score);
            }

            return SolvedCount.CompareTo(other.SolvedCount);
        }
    }
}

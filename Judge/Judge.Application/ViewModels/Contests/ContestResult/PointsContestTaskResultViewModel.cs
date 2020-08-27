using System;

namespace Judge.Application.ViewModels.Contests.ContestResult
{
    public class PointsContestTaskResultViewModel : ContestTaskResultViewModelBase
    {
        private const int MaxScore = 100;
        private const int Penalty = 20;
        private const int MinScore = 5;

        public PointsContestTaskResultViewModel(DateTime submitDateUtc)
            : base(submitDateUtc)
        {
        }

        public override string GetLabel()
        {
            return Solved ? GetScore().ToString() : string.Empty;
        }

        public override int GetScore()
        {
            if (!Solved) return 0;

            var result = MaxScore - ((Attempts - 1) * Penalty);
            return result <= 0 ? MinScore : result;

        }
    }
}

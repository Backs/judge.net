﻿namespace Judge.Application.ViewModels.Contests.ContestResult
{
    public sealed class AcmContestUserResultViewModel : ContestUserResultViewModelBase
    {
        public override int CompareTo(object other)
        {
            var value = (AcmContestUserResultViewModel)other;
            if (this.SolvedCount == value.SolvedCount)
            {
                return value.Score.CompareTo(this.Score);
            }

            return this.SolvedCount.CompareTo(value.SolvedCount);
        }
    }
}

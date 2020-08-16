using System;

namespace Judge.Application.ViewModels.Contests.ContestResult
{
    public sealed class AcmContestTaskResultViewModel : ContestTaskResultViewModelBase
    {
        public AcmContestTaskResultViewModel(DateTime contestStartTime, DateTime submitDateUtc)
        : base(submitDateUtc)
        {
            _elapsedTime = submitDateUtc - contestStartTime;
        }

        private TimeSpan _elapsedTime;

        public override string GetLabel()
        {
            var hours = (int)_elapsedTime.TotalHours;
            var minutes = _elapsedTime.Minutes;
            return $"{hours}:{minutes:00}";
        }

        public override int GetScore()
        {
            if (!Solved)
                return 0;

            return (Attempts - 1) * 20 + (int)_elapsedTime.TotalMinutes;
        }
    }
}

namespace Judge.Application.ViewModels.Contests.ContestResult
{
    using System;

    public sealed class AcmContestTaskResultViewModel : ContestTaskResultViewModelBase
    {
        public AcmContestTaskResultViewModel(DateTime contestStartTime, DateTime submitDateUtc)
        : base(submitDateUtc)
        {
            this.elapsedTime = submitDateUtc - contestStartTime;
        }

        private TimeSpan elapsedTime;

        public override string GetLabel()
        {
            var hours = (int)this.elapsedTime.TotalHours;
            var minutes = this.elapsedTime.Minutes;
            return $"{hours}:{minutes:00}";
        }

        public override int GetScore()
        {
            if (!this.Solved)
                return 0;

            return (this.Attempts - 1) * 20 + (int)this.elapsedTime.TotalMinutes;
        }
    }
}

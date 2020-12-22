namespace Judge.Application.ViewModels.Contests.ContestResult
{
    using System;

    public sealed class FixedDateContestTaskResultViewModel : ContestTaskResultViewModelBase
    {
        private readonly DateTime checkPointDate;

        public FixedDateContestTaskResultViewModel(DateTime submitDateUtc, DateTime checkPointDate)
        : base(submitDateUtc)
        {
            this.checkPointDate = checkPointDate;
        }

        public override string GetLabel()
        {
            var diff = this.SubmitDateUtc.Subtract(this.checkPointDate).Duration();

            var hours = (int)diff.TotalHours;
            var minutes = diff.Minutes;
            return $"{hours}:{minutes:00}";
        }

        public override int GetScore()
        {
            var diff = this.SubmitDateUtc.Subtract(this.checkPointDate).Duration();
            return (int)diff.TotalMinutes;
        }
    }
}

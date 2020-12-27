namespace Judge.Application.ViewModels.Contests.ContestResult
{
    using System;

    public sealed class CheckPointContestTaskResultViewModel : ContestTaskResultViewModelBase
    {
        private readonly DateTime checkPointDate;

        public CheckPointContestTaskResultViewModel(DateTime submitDateUtc, DateTime checkPointDate)
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
            if (!this.Solved)
                return 0;

            var diff = this.SubmitDateUtc.Subtract(this.checkPointDate).Duration();
            return (this.Attempts - 1) * 20 + (int)diff.TotalMinutes;
        }
    }
}

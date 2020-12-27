namespace Judge.Application.ViewModels.Contests.ContestResult
{
    using System;

    public abstract class ContestTaskResultViewModelBase
    {
        protected ContestTaskResultViewModelBase(DateTime submitDateUtc)
        {
            this.SubmitDateUtc = submitDateUtc;
        }

        public bool Solved { get; set; }
        public long ProblemId { get; set; }
        public int Attempts { get; set; }
        public DateTime SubmitDateUtc { get; }
        public abstract string GetLabel();
        public abstract int GetScore();

        public string GetAttempts()
        {
            if (!this.Solved)
            {
                return "-" + this.Attempts;
            }

            var additionalAttempts = this.Attempts - 1;
            return additionalAttempts == 0 ? "+" : "+" + additionalAttempts;
        }
    }
}

using System;

namespace Judge.Application.ViewModels.Contests.ContestResult
{
    public abstract class ContestTaskResultViewModelBase
    {
        protected ContestTaskResultViewModelBase(DateTime submitDateUtc)
        {
            SubmitDateUtc = submitDateUtc;
        }

        public bool Solved { get; set; }
        public long ProblemId { get; set; }
        public int Attempts { get; set; }
        public DateTime SubmitDateUtc { get; }
        public abstract string GetLabel();
        public abstract int GetScore();

        public string GetAttempts()
        {
            if (Solved)
            {
                var a = Attempts - 1;
                if (a == 0)
                {
                    return "+";
                }
                return "+" + a;
            }
            return "-" + Attempts;
        }
    }
}
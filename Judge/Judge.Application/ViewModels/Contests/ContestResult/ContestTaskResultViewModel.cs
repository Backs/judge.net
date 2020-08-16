using System;

namespace Judge.Application.ViewModels.Contests.ContestResult
{
    public sealed class ContestTaskResultViewModel
    {
        public bool Solved { get; set; }
        public long ProblemId { get; set; }
        public int Attempts { get; set; }
        public DateTime SubmitDateUtc { get; }

        public ContestTaskResultViewModel(DateTime contestStartTime, DateTime submitDateUts)
        {
            _elapsedTime = submitDateUts - contestStartTime;
            SubmitDateUtc = submitDateUts;
        }

        private TimeSpan _elapsedTime;

        public string GetScoreLabel()
        {
            var hours = (int)_elapsedTime.TotalHours;
            var minutes = _elapsedTime.Minutes;
            return $"{hours}:{minutes:00}";
        }

        public int GetScore()
        {
            if (!Solved)
                return 0;

            return (Attempts - 1) * 20 + (int)_elapsedTime.TotalMinutes;
        }

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

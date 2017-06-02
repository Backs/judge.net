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

        private string GetElapsedTime()
        {
            var minutes = (int)_elapsedTime.TotalMinutes;
            var seconds = _elapsedTime.Seconds;
            return $"{minutes}:{seconds:00}";
        }

        public override string ToString()
        {
            string result;
            if (Solved)
            {
                var a = Attempts - 1;
                if (a == 0)
                {
                    result = "+";
                }
                else
                {

                    result = "+" + a;
                }
            }
            else
            {
                result = "-" + Attempts;
            }

            result = result + $"({GetElapsedTime()})";

            return result;
        }
    }
}

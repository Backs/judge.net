using System;
using Judge.Model.Contests;

namespace Judge.Application.ViewModels.Contests.ContestsList
{
    public sealed class ContestItem
    {
        public ContestItem(Contest contest)
        {
            Id = contest.Id;
            Name = contest.Name;
            StartTime = contest.StartTime;
            Duration = contest.FinishTime - contest.StartTime;
            FinishTime = contest.FinishTime;

            var now = DateTime.UtcNow;

            if (now < contest.StartTime)
            {
                Status = ContestStatus.Planned;
            }
            else if (now < contest.FinishTime)
            {
                Status = ContestStatus.Started;
            }
            else
            {
                Status = ContestStatus.Finished;
            }
        }

        public DateTime FinishTime { get; set; }

        public string TextStatus
        {
            get
            {
                switch (Status)
                {
                    case ContestStatus.Planned:
                        return Resources.ContestStatusPlanned;
                    case ContestStatus.Started:
                        return Resources.ContestStatusStarted;
                    case ContestStatus.Finished:
                        return Resources.ContestStatusFinished;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public string GetLeftTimeString()
        {
            if (Status == ContestStatus.Started)
            {
                var time = FinishTime - DateTime.UtcNow;
                var hours = (int)time.TotalHours;
                var minutes = time.Minutes;
                return $"{Resources.Left}: {hours}:{minutes:00}";
            }
            return string.Empty;
        }

        public string GetDurationTimeString()
        {
            var hours = (int)Duration.TotalHours;
            var minutes = Duration.Minutes;
            return $"{hours}:{minutes:00}";
        }

        public int Id { get; }
        public string Name { get; }
        public DateTime StartTime { get; }
        public TimeSpan Duration { get; }
        public ContestStatus Status { get; }
        public bool IsFinished => Status == ContestStatus.Finished;
        public bool IsNotStarted => Status == ContestStatus.Planned;
        public bool IsStarted => Status == ContestStatus.Started;
    }
}

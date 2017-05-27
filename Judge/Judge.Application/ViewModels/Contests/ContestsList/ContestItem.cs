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

        public int Id { get; }
        public string Name { get; }
        public DateTime StartTime { get; }
        public TimeSpan Duration { get; }
        public ContestStatus Status { get; }
    }
}

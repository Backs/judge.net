namespace Judge.Application.ViewModels.Contests.ContestsList
{
    using System;
    using Judge.Model.Contests;
    using ContestRules = Judge.Application.ViewModels.Admin.Contests.ContestRules;

    public sealed class ContestItem
    {
        public ContestItem(Contest contest)
        {
            this.Id = contest.Id;
            this.Name = contest.Name;
            this.StartTime = contest.StartTime;
            this.Duration = contest.FinishTime - contest.StartTime;
            this.FinishTime = contest.FinishTime;
            this.Rules = (ContestRules)contest.Rules;
            this.CheckPointTime = contest.CheckPointTime;

            var now = DateTime.UtcNow;

            if (now < contest.StartTime)
            {
                this.Status = ContestStatus.Planned;
            }
            else if (now < contest.FinishTime)
            {
                this.Status = ContestStatus.Started;
            }
            else
            {
                this.Status = ContestStatus.Finished;
            }
        }

        public DateTime? CheckPointTime { get; set; }

        public ContestRules Rules { get; set; }

        public DateTime FinishTime { get; set; }

        public string TextStatus
        {
            get
            {
                switch (this.Status)
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
            if (this.Status == ContestStatus.Started)
            {
                var time = this.FinishTime - DateTime.UtcNow;
                var hours = (int)time.TotalHours;
                var minutes = time.Minutes;
                return $"{Resources.Left}: {hours}:{minutes:00}";
            }
            return string.Empty;
        }

        public string GetDurationTimeString()
        {
            var hours = (int)this.Duration.TotalHours;
            var minutes = this.Duration.Minutes;
            return $"{hours}:{minutes:00}";
        }

        public int Id { get; }
        public string Name { get; }
        public DateTime StartTime { get; }
        public TimeSpan Duration { get; }
        public ContestStatus Status { get; }
        public bool IsFinished => this.Status == ContestStatus.Finished;
        public bool IsNotStarted => this.Status == ContestStatus.Planned;
        public bool IsStarted => this.Status == ContestStatus.Started;
        public bool HasAdditionalRules => this.Rules == ContestRules.CheckPoint;

        public string GetAdditionalRules()
        {
            if (this.HasAdditionalRules)
            {
                if (this.Rules == ContestRules.CheckPoint)
                {
                    return $"Время, относительно которого считается штраф: {this.CheckPointTime}";
                }
            }

            return null;
        }
    }
}

using System;

namespace Judge.Application.ViewModels.Contests.ContestsList
{
    public sealed class ContestItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public TimeSpan Duration { get; set; }
    }
}

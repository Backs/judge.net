using System;
using System.Collections.Generic;

namespace Judge.Model.Entities.Contests
{
    public sealed class Contest
    {
        public Contest()
        {
            Tasks = new HashSet<Task>();
        }
        public Contest(DateTime startTime, DateTime endTime, string name)
        {
            if (startTime >= endTime)
                throw new InvalidOperationException();

            if (string.IsNullOrWhiteSpace(name))
                throw new NullReferenceException("name");

            StartTime = startTime;
            EndTime = endTime;
            Name = name;
        }

        public string Name { get; private set; }
        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }
        public ICollection<Task> Tasks { get; private set; }
    }
}

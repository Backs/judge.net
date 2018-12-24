using System;

namespace Judge.Model.Contests
{
    public sealed class Contest
    {
        public int Id { get; private set; }
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime FinishTime { get; set; }
        public DateTime? FreezeTime { get; set; }
        public bool IsOpened { get; set; }

        public override string ToString()
        {
            return $"{Id}-{Name}";
        }
    }
}

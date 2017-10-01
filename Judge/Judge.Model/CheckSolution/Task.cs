using System;

namespace Judge.Model.CheckSolution
{
    public sealed class Task
    {
        private string _name;
        public long Id { get; private set; }
        public string TestsFolder { get; set; }
        public int TimeLimitMilliseconds { get; set; }
        public int MemoryLimitBytes { get; set; }

        public string Name
        {
            get { return _name; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));
                _name = value;
            }
        }

        public DateTime CreationDateUtc { get; private set; }
        public string Statement { get; set; }
        public bool IsOpened { get; set; }
    }
}

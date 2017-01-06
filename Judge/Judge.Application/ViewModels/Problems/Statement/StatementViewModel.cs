using System;

namespace Judge.Application.ViewModels.Problems.Statement
{
    public sealed class StatementViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Statement { get; set; }
        public int TimeLimitMilliseconds { get; set; }
        public int MemoryLimitBytes { get; set; }
        public DateTime CreationDate { get; set; }
    }
}

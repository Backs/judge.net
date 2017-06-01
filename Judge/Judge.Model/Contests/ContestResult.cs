using System;
using System.Collections.Generic;

namespace Judge.Model.Contests
{
    public sealed class ContestResult
    {
        public long UserId { get; set; }
        public IEnumerable<ContestTaskResult> TaskResults { get; set; }
    }
}

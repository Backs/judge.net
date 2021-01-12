namespace Judge.Model.Contests
{
    using System.Collections.Generic;

    public sealed class ContestResult
    {
        public long UserId { get; set; }

        public IEnumerable<ContestTaskResult> TaskResults { get; set; }
    }
}

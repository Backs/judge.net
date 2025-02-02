namespace Judge.Model.Contests
{
    using System.Collections.Generic;

    public sealed class ContestResult
    {
        public long UserId { get; set; }

        public ContestTaskResult[] TaskResults { get; set; }
    }
}

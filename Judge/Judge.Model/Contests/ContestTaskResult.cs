namespace Judge.Model.Contests
{
    using System;

    public sealed class ContestTaskResult
    {
        public long ProblemId { get; set; }

        public int Attempts { get; set; }

        public bool Solved { get; set; }

        public DateTime SubmitDateUtc { get; set; }
    }
}

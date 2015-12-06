using System;

namespace Judge.Model.Entities
{
    public sealed class Submit
    {
        public DateTime SubmitTime { get; set; }
        public long ProblemId { get; set; }
        public long UserId { get; set; }
        public long? ContestId { get; set; }
    }
}

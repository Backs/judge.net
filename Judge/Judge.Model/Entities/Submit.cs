using System;

namespace Judge.Model.Entities
{
    public sealed class Submit
    {
        public long Id { get; set; }
        public DateTime SubmitTime { get; set; }
        public long ProblemId { get; set; }
        public long UserId { get; set; }
        public long? ContestId { get; set; }
        public int LanguageId { get; set; }
        public string SourceCode { get; set; }
        public SubmitResult Result { get; set; }
    }
}

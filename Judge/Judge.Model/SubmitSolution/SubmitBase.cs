using System;
using System.Collections.Generic;

namespace Judge.Model.SubmitSolution
{
    public abstract class SubmitBase
    {
        public long UserId { get; set; }
        public string FileName { get; set; }
        public int LanguageId { get; set; }
        public string SourceCode { get; set; }
        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        public long Id { get; internal set; }
        public ICollection<SubmitResult> Results { get; } = new HashSet<SubmitResult>();

        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        public DateTime SubmitDateUtc { get; private set; }
        public long ProblemId { get; set; }
        public string UserHost { get; set; }
        public string SessionId { get; set; }
    }
}
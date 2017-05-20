using System;
using System.Collections.Generic;

namespace Judge.Model.SubmitSolution
{
    public abstract class SubmitBase
    {
        protected SubmitBase()
        {
            Results = new HashSet<SubmitResult>();
        }
        public long UserId { get; set; }
        public string FileName { get; set; }
        public int LanguageId { get; set; }
        public string SourceCode { get; set; }
        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        public long Id { get; internal set; }
        public ICollection<SubmitResult> Results { get; }
        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        public DateTime SubmitDateUtc { get; private set; }

        public abstract long GetProblemId();
    }
}
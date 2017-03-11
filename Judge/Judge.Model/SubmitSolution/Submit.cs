using System;
using System.Collections.Generic;

namespace Judge.Model.SubmitSolution
{
    public sealed class Submit
    {
        private Submit()
        {
            Results = new HashSet<SubmitResult>();
        }

        public static Submit Create()
        {
            var submit = new Submit();
            submit.Results.Add(new SubmitResult(submit));
            return submit;
        }

        public long UserId { get; set; }
        public long ProblemId { get; set; }
        public string FileName { get; set; }
        public int LanguageId { get; set; }
        public string SourceCode { get; set; }
        public long Id { get; private set; }
        public ICollection<SubmitResult> Results { get; }
        public DateTime SubmitDateUtc { get; private set; }
    }
}

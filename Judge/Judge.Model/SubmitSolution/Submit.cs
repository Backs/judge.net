using System.Collections.Generic;

namespace Judge.Model.SubmitSolution
{
    public sealed class Submit
    {
        public Submit()
        {
            CheckQueueItem = new CheckQueue();
        }

        public long UserId { get; set; }
        public long ProblemId { get; set; }
        public string FileName { get; set; }
        public int LanguageId { get; set; }
        public string SourceCode { get; set; }
        public long Id { get; private set; }
        public CheckQueue CheckQueueItem { get; private set; }
    }
}

namespace Judge.Model.SubmitSolution
{
    public sealed class SubmitResult
    {
        private SubmitResult()
        {
            
        }
        public SubmitResult(Submit submit)
        {
            CheckQueue = new CheckQueue();
            Status = SubmitStatus.Pending;
            Submit = submit;
        }

        public long Id { get; private set; }
        public SubmitStatus Status { get; private set; }
        public CheckQueue CheckQueue { get; private set; }
        public Submit Submit { get; private set; }
        public int? PassedTests { get; set; }
        public int? TotalBytes { get; set; }
        public int? TotalMilliseconds { get; set; }
    }
}

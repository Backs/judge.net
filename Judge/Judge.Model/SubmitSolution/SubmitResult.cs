namespace Judge.Model.SubmitSolution
{
    public sealed class SubmitResult
    {
        public SubmitResult()
        {
            CheckQueue = new CheckQueue();
            Status = SubmitStatus.Pending;
        }

        public long Id { get; private set; }
        public SubmitStatus Status { get; private set; }
        public CheckQueue CheckQueue { get; private set; }
    }
}

namespace Judge.Model.Entities
{
    public enum SubmitStatus
    {
        None,
        Pending,
        CompilationError,
        RuntimeError,
        TimeLimitExceeded,
        MemoryLimitExceeded,
        WrongAnswer,
        Accepted
    }
}

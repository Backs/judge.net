namespace Judge.Model.Entities
{
    public enum SubmitStatus
    {
        None,
        CompilationError,
        RuntimeError,
        TimeLimitExceeded,
        MemoryLimitExceeded,
        WrongAnswer,
        Accepted
    }
}

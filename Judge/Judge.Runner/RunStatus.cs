namespace Judge.Runner
{
    public enum RunStatus
    {
        Success,
        TimeLimitExceeded,
        MemoryLimitExceeded,
        SecurityViolation,
        RuntimeError,
        InvocationFailed
    }
}

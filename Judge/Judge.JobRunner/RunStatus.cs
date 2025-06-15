namespace Judge.JobRunner;

public enum RunStatus
{
    Success,
    TimeLimitExceeded,
    MemoryLimitExceeded,
    SecurityViolation,
    RuntimeError,
    InvocationFailed,
    IdlenessLimitExceeded
}
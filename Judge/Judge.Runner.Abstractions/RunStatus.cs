namespace Judge.Runner.Abstractions;

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
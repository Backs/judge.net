namespace Judge.Web.Client.Submits;

public enum SubmitStatus
{
    Pending,
    CompilationError,
    RuntimeError,
    TimeLimitExceeded,
    MemoryLimitExceeded,
    WrongAnswer,
    Accepted,
    ServerError,

    TooEarly,
    Unpolite,
    TooManyLines
}
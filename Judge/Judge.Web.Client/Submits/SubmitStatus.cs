namespace Judge.Web.Client.Submits;

public enum SubmitStatus
{
    /// <summary>
    /// Pending to check
    /// </summary>
    Pending,

    /// <summary>
    /// Compilation error
    /// </summary>
    CompilationError,

    /// <summary>
    /// Runtime error
    /// </summary>
    RuntimeError,

    /// <summary>
    /// Time limit exceeded
    /// </summary>
    TimeLimitExceeded,

    /// <summary>
    /// Memory limit exceeded
    /// </summary>
    MemoryLimitExceeded,

    /// <summary>
    /// Wrong answer
    /// </summary>
    WrongAnswer,

    /// <summary>
    /// Accepted
    /// </summary>
    Accepted,

    /// <summary>
    /// Server error
    /// </summary>
    ServerError,

    /// <summary>
    /// Too early submit
    /// </summary>
    /// <remarks>Use for fun contest</remarks>
    TooEarly,

    /// <summary>
    /// Unpolite
    /// </summary>
    /// <remarks>Use for fun contest</remarks>
    Unpolite,

    /// <summary>
    /// Too many lines
    /// </summary>
    /// <remarks>Use for fun contest</remarks>
    TooManyLines,

    /// <summary>
    /// Wrong language
    /// </summary>
    /// <remarks>Use for fun contest</remarks>
    WrongLanguage,

    /// <summary>
    /// Presentation error
    /// </summary>
    PresentationError,

    /// <summary>
    /// PR not found
    /// </summary>
    PRNotFound,

    /// <summary>
    /// Login not found
    /// </summary>
    LoginNotFound,
}
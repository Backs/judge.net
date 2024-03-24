namespace Judge.Model.SubmitSolution
{
    public enum SubmitStatus
    {
        Pending = 0,
        CompilationError = 1,
        RuntimeError = 2,
        TimeLimitExceeded = 3,
        MemoryLimitExceeded = 4,
        WrongAnswer = 5,
        Accepted = 6,
        ServerError = 7,
        PresentationError,

        TooEarly = 9900,
        Unpolite = 9901,
        TooManyLines = 9902,
        WrongLanguage = 9903
    }
}
namespace Judge.Checker;

public enum CheckStatus
{
    OK = 0,
    WA = 1,
    PE = 2,
    Fail = 3,
    Dirt = 4,
    Points = 5,
    UnexpectedEOF = 8,
    Partially = 16,

    TooEarly = 9900,
    Unpolite = 9901,
    TooManyLines = 9902,
    WrongLanguage = 9903,
    PullRequestNotFound = 9904,
    LoginNotFound = 9905,
}
namespace Judge.Checker;

public record FileOptions(
    string WorkingDirectory,
    string InputFileName,
    string OutputFileName,
    string AnswerFileName);
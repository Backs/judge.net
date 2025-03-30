import {SubmitStatus} from "../api/Api.ts";

export const getColor = (status: SubmitStatus) => {
    switch (status) {
        case SubmitStatus.CompilationError:
        case SubmitStatus.ServerError:
            return "warning";
        case SubmitStatus.Accepted:
            return "success";
        case SubmitStatus.Pending:
            return "processing";
    }
    return "error";
}

export const getStatusText = (status: SubmitStatus) => {
    switch (status) {
        case SubmitStatus.WrongLanguage:
            return "Wrong language";
        case SubmitStatus.PresentationError:
            return "Presentation error";
        case SubmitStatus.Pending:
            return "Pending...";
        case SubmitStatus.CompilationError:
            return "Compilation error";
        case SubmitStatus.RuntimeError:
            return "Runtime error";
        case SubmitStatus.TimeLimitExceeded:
            return "Time limit exceeded";
        case SubmitStatus.MemoryLimitExceeded:
            return "Memory limit exceeded";
        case SubmitStatus.WrongAnswer:
            return "Wrong answer";
        case SubmitStatus.Accepted:
            return "Accepted";
        case SubmitStatus.ServerError:
            return "Server error";
        case SubmitStatus.TooEarly:
            return "Too early submit";
        case SubmitStatus.Unpolite:
            return "Unpolite solution";
        case SubmitStatus.TooManyLines:
            return "Too many lines for such a simple task";
        case SubmitStatus.PRNotFound:
            return "PR not found";
        case SubmitStatus.LoginNotFound:
            return "Your login not found";
        case SubmitStatus.NotSolvedYet:
            return "Not solved yet";
    }

    return status;
}
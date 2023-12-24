namespace Judge.Services;

public enum AuthenticationResult
{
    Unknown,
    UserNotFound,
    PasswordVerificationFailed,
    Success
}
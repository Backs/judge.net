using System;
using Judge.Web.Client.Users;

namespace Judge.Services;

public record Authentication(AuthenticationResult Result, User? User, string[] Roles)
{
    public static Authentication UserNotFound() =>
        new Authentication(AuthenticationResult.UserNotFound, null, Array.Empty<string>());

    public static Authentication Success(User user, string[] roles) =>
        new Authentication(AuthenticationResult.Success, user, roles);

    public static Authentication PasswordVerificationFailed() =>
        new Authentication(AuthenticationResult.PasswordVerificationFailed, null, Array.Empty<string>());
}
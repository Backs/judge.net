using System.Security;

namespace Judge.Runner.Abstractions;

public record UserCredentials(string UserName, string? Domain, SecureString Password);
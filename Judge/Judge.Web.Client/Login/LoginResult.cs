using System.ComponentModel.DataAnnotations;

namespace Judge.Web.Client.Login;

/// <summary>
/// Login result
/// </summary>
public class LoginResult
{
    /// <summary>
    /// Authentication token
    /// </summary>
    [Required]
    public string Token { get; set; } = null!;
}
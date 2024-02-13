using System.ComponentModel.DataAnnotations;

namespace Judge.Web.Client.Login;

/// <summary>
/// Login information
/// </summary>
public class Login
{
    /// <summary>
    /// User email
    /// </summary>
    [Required]
    public string Email { get; set; } = null!;

    /// <summary>
    /// User password
    /// </summary>
    [Required]
    public string Password { get; set; } = null!;
}
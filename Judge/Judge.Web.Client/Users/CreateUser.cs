using System.ComponentModel.DataAnnotations;

namespace Judge.Web.Client.Users;

/// <summary>
/// Create new user
/// </summary>
public class CreateUser
{
    /// <summary>
    /// User email 
    /// </summary>
    [Required]
    [MinLength(2)]
    [EmailAddress]
    public string Email { get; set; } = null!;

    /// <summary>
    /// User login
    /// </summary>
    [Required]
    [MinLength(2)]
    public string Login { get; set; } = null!;

    /// <summary>
    /// User password
    /// </summary>
    [Required]
    [MinLength(6)]
    public string Password { get; set; } = null!;
}
using System.ComponentModel.DataAnnotations;

namespace Judge.Web.Client.Users;

/// <summary>
/// User information
/// </summary>
public sealed class User
{
    /// <summary>
    /// User id
    /// </summary>
    [Required]
    public long Id { get; set; }
    
    /// <summary>
    /// User login
    /// </summary>
    [Required]
    public string Login { get; set; } = null!;
    
    /// <summary>
    /// User email
    /// </summary>
    [Required]
    public string Email { get; set; } = null!;
}
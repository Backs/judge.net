using System.ComponentModel.DataAnnotations;

namespace Judge.Web.Client.Users;

public class CreateUser
{
    [Required]
    [MinLength(2)]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [MinLength(2)]
    public string Login { get; set; } = null!;

    [MinLength(6)]
    public string Password { get; set; } = null!;
}
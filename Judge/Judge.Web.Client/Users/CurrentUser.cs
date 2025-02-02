using System.ComponentModel.DataAnnotations;

namespace Judge.Web.Client.Users;

public class CurrentUser : User
{
    /// <summary>
    /// User roles
    /// </summary>
    [Required]
    public string[] Roles { get; set; } = [];
}
using System.ComponentModel.DataAnnotations;

namespace Judge.Web.Client.Users;

public class UsersQuery
{
    /// <summary>
    /// User name or email
    /// </summary>
    [MinLength(2)]
    public string? Name { get; set; }
    
    /// <summary>
    /// Users to skip
    /// </summary>
    [Range(0, int.MaxValue)]
    public int Skip { get; set; }
    
    /// <summary>
    /// Users to take
    /// </summary>
    [Range(1, 100)]
    public int Take { get; set; } = 20;
}
using System.ComponentModel.DataAnnotations;

namespace Judge.Web.Client.Users;

public class UsersQuery
{
    [MinLength(2)]
    public string? Name { get; set; }
    
    [Range(0, int.MaxValue)]
    public int Skip { get; set; }

    [Range(1, 100)]
    public int Take { get; set; } = 20;
}
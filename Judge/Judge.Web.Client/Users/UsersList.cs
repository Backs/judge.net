using System;
using System.ComponentModel.DataAnnotations;

namespace Judge.Web.Client.Users;

/// <summary>
/// Users list
/// </summary>
public class UsersList
{
    /// <summary>
    /// Users
    /// </summary>
    [Required]
    public User[] Items { get; set; } = Array.Empty<User>();
    
    /// <summary>
    /// Total users count
    /// </summary>
    [Required]
    public int TotalCount { get; set; }
}
using System;

namespace Judge.Web.Client.Users;

/// <summary>
/// Users list
/// </summary>
public class UsersList
{
    /// <summary>
    /// Users
    /// </summary>
    public User[] Items { get; set; } = Array.Empty<User>();
    
    /// <summary>
    /// Total users count
    /// </summary>
    public int TotalCount { get; set; }
}
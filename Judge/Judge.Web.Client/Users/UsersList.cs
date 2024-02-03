using System;

namespace Judge.Web.Client.Users;

public class UsersList
{
    public User[] Items { get; set; } = Array.Empty<User>();
    public int TotalCount { get; set; }
}
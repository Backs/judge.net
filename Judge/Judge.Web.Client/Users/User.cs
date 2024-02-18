namespace Judge.Web.Client.Users;

/// <summary>
/// User information
/// </summary>
public sealed class User
{
    /// <summary>
    /// User id
    /// </summary>
    public long Id { get; set; }
    
    /// <summary>
    /// User login
    /// </summary>
    public string Login { get; set; } = null!;
    
    /// <summary>
    /// User email
    /// </summary>
    public string Email { get; set; } = null!;
}
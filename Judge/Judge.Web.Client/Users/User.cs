namespace Judge.Web.Client.Users;

public sealed class User
{
    public long Id { get; set; }
    public string Login { get; set; } = null!;
    public string Email { get; set; } = null!;
}
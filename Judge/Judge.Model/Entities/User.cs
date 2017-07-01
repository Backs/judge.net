using Microsoft.AspNet.Identity;

namespace Judge.Model.Entities
{
    public sealed class User : IUser<long>
    {
        public long Id { get; internal set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
    }
}

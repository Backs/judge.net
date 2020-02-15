using System.Linq;
using Judge.Model.Entities;
using Microsoft.AspNet.Identity;

namespace Judge.Application
{
    public sealed class ApplicationUser : IUser<long>
    {
        public User User { get; }

        public ApplicationUser(User user)
        {
            User = user;
        }

        public ApplicationUser()
        {
            User = new User();
        }

        public long Id => User.Id;
        public string UserName
        {
            get => User.UserName;
            set => User.UserName = value;
        }

        public string Email
        {
            get => User.Email;
            set => User.Email = value;
        }

        public string PasswordHash
        {
            get => User.PasswordHash;
            set => User.PasswordHash = value;
        }

        public string[] UserRoles => User.UserRoles.Select(o => o.RoleName).ToArray();
    }
}

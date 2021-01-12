namespace Judge.Application
{
    using System.Linq;
    using Judge.Model.Entities;
    using Microsoft.AspNet.Identity;

    public sealed class ApplicationUser : IUser<long>
    {
        public User User { get; }

        public ApplicationUser(User user)
        {
            this.User = user;
        }

        public ApplicationUser()
        {
            this.User = new User();
        }

        public long Id => this.User.Id;
        public string UserName
        {
            get => this.User.UserName;
            set => this.User.UserName = value;
        }

        public string Email
        {
            get => this.User.Email;
            set => this.User.Email = value;
        }

        public string PasswordHash
        {
            get => this.User.PasswordHash;
            set => this.User.PasswordHash = value;
        }

        public string[] UserRoles => this.User.UserRoles.Select(o => o.RoleName).ToArray();
    }
}

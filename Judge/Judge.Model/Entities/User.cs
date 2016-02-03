using Microsoft.AspNet.Identity;

namespace Judge.Model.Entities
{
    public class User : IUser<long>
    {
        public long Id { get; private set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
    }
}

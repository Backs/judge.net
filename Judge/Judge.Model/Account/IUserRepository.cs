using System.Collections.Generic;
using Judge.Model.Entities;

namespace Judge.Model.Account
{
    public interface IUserRepository
    {
        IEnumerable<User> GetUsers(IEnumerable<long> users);
        User GetUser(long id);
        void Add(User user);
        void Update(User user);
        void Delete(User user);
        User GetUser(string userName);
    }
}
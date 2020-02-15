using System.Collections.Generic;
using Judge.Model.Entities;

namespace Judge.Model.Account
{
    public interface IUserRepository
    {
        User Get(long id);
        void Add(User user);
        void Update(User user);
        void Delete(User user);
        User Get(string userName);
        IEnumerable<User> Find(ISpecification<User> specification);
    }
}
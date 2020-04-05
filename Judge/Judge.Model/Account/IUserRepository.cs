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
        
        User FindByName(string userName);

        User FindByEmail(string email);

        IEnumerable<User> Find(ISpecification<User> specification);
    }
}
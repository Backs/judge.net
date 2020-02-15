using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Judge.Model;
using Judge.Model.Account;
using Judge.Model.Entities;

namespace Judge.Data.Repository
{
    internal sealed class UserRepository : IUserRepository
    {
        private readonly DataContext _context;

        public UserRepository(DataContext context)
        {
            _context = context;
        }

        public User Get(long id)
        {
            return _context.Set<User>().Include(o => o.UserRoles).FirstOrDefault(o => o.Id == id);
        }

        public void Add(User user)
        {
            _context.Set<User>().Add(user);
        }

        public void Update(User user)
        {
            _context.Entry(user).State = EntityState.Modified;
        }

        public void Delete(User user)
        {
            _context.Set<User>().Remove(user);
        }

        public User Get(string userName)
        {
            return _context.Set<User>().Include(o => o.UserRoles).FirstOrDefault(o => o.Email == userName);
        }

        public IEnumerable<User> Find(ISpecification<User> specification)
        {
            return _context.Set<User>().Include(o => o.UserRoles).Where(specification.IsSatisfiedBy);
        }
    }
}

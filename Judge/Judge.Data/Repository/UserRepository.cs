using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
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

        public IEnumerable<User> GetUsers(IEnumerable<long> users)
        {
            return _context.Set<User>().Include(o => o.UserRoles).Where(o => users.Contains(o.Id)).OrderBy(o => o.Id).AsEnumerable();
        }

        public User GetUser(long id)
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

        public User GetUser(string userName)
        {
            return _context.Set<User>().Include(o => o.UserRoles).FirstOrDefault(o => o.Email == userName);
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using Judge.Model;
using Judge.Model.Account;
using Judge.Model.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

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
            return BaseQuery().FirstOrDefault(o => o.Id == id);
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

        public User FindByName(string userName)
        {
            return BaseQuery().FirstOrDefault(o => o.UserName == userName);
        }

        public User FindByEmail(string email)
        {
            return BaseQuery().FirstOrDefault(o => o.Email == email);
        }

        public IEnumerable<User> Find(ISpecification<User> specification)
        {
            return BaseQuery().Where(specification.IsSatisfiedBy);
        }

        private IIncludableQueryable<User, ICollection<UserRole>> BaseQuery()
        {
            return _context.Set<User>().Include(o => o.UserRoles);
        }
    }
}

using System.Data.Entity;
using System.Threading.Tasks;
using Judge.Model.Entities;
using Microsoft.AspNet.Identity;

namespace Judge.Data
{
    internal sealed class UserStore : IUserPasswordStore<User, long>
    {
        private readonly DataContext _context;
        private readonly DbSet<User> _dbSet;

        public UserStore(string connectionString)
        {
            _context = new DataContext(connectionString);
            _dbSet = _context.Set<User>();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public Task CreateAsync(User user)
        {
            _dbSet.Add(user);
            return _context.SaveChangesAsync();
        }

        public Task UpdateAsync(User user)
        {
            _context.Entry(user).State = EntityState.Modified;
            return _context.SaveChangesAsync();
        }

        public Task DeleteAsync(User user)
        {
            _dbSet.Remove(user);
            return _context.SaveChangesAsync();
        }

        public Task<User> FindByIdAsync(long userId)
        {
            return _dbSet.SingleOrDefaultAsync(o => o.Id == userId);
        }

        public Task<User> FindByNameAsync(string userName)
        {
            return _dbSet.SingleOrDefaultAsync(o => o.UserName == userName);
        }

        public Task SetPasswordHashAsync(User user, string passwordHash)
        {
            return Task.FromResult(user.PasswordHash = passwordHash);
        }

        public Task<string> GetPasswordHashAsync(User user)
        {
            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(User user)
        {
            return Task.FromResult(!string.IsNullOrWhiteSpace(user.PasswordHash));
        }
    }
}

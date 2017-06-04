using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Judge.Model.Account;
using Judge.Model.Entities;
using Microsoft.AspNet.Identity;

namespace Judge.Data
{
    internal sealed class UserStore : IUserPasswordStore<User, long>, IUserLockoutStore<User, long>, IUserTwoFactorStore<User, long>, IUserRepository
    {
        private readonly DataContext _context;
        private readonly DbSet<User> _dbSet;

        public UserStore(DataContext context)
        {
            _context = context;
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
            return _dbSet.SingleOrDefaultAsync(o => o.Email == userName);
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

        public Task<DateTimeOffset> GetLockoutEndDateAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task SetLockoutEndDateAsync(User user, DateTimeOffset lockoutEnd)
        {
            throw new NotImplementedException();
        }

        public Task<int> IncrementAccessFailedCountAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task ResetAccessFailedCountAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetAccessFailedCountAsync(User user)
        {
            return Task.FromResult(0);
        }

        public Task<bool> GetLockoutEnabledAsync(User user)
        {
            return Task.FromResult(false);
        }

        public Task SetLockoutEnabledAsync(User user, bool enabled)
        {
            throw new NotImplementedException();
        }

        public Task SetTwoFactorEnabledAsync(User user, bool enabled)
        {
            throw new NotImplementedException();
        }

        public Task<bool> GetTwoFactorEnabledAsync(User user)
        {
            return Task.FromResult(false);
        }

        public IEnumerable<User> GetUsers(IEnumerable<long> users)
        {
            return _dbSet.Where(o => users.Contains(o.Id)).OrderBy(o => o.Id).AsEnumerable();
        }
    }
}

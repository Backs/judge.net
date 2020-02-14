using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Judge.Data;
using Judge.Model.Account;
using Judge.Model.Entities;
using Microsoft.AspNet.Identity;

namespace Judge.Application
{
    internal sealed class UserStore : IUserPasswordStore<User, long>, IUserLockoutStore<User, long>, IUserTwoFactorStore<User, long>, IUserRoleStore<User, long>
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        public UserStore(IUnitOfWorkFactory unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        public void Dispose()
        {

        }

        public Task CreateAsync(User user)
        {
            using (var uow = _unitOfWorkFactory.GetUnitOfWork(true))
            {
                var userRepository = uow.GetRepository<IUserRepository>();
                userRepository.Add(user);
            }

            return Task.CompletedTask;
        }

        public Task UpdateAsync(User user)
        {
            using (var uow = _unitOfWorkFactory.GetUnitOfWork(true))
            {
                var userRepository = uow.GetRepository<IUserRepository>();
                userRepository.Update(user);
            }

            return Task.CompletedTask;
        }

        public Task DeleteAsync(User user)
        {
            using (var uow = _unitOfWorkFactory.GetUnitOfWork(true))
            {
                var userRepository = uow.GetRepository<IUserRepository>();
                userRepository.Delete(user);
            }

            return Task.CompletedTask;
        }

        public Task<User> FindByIdAsync(long userId)
        {
            using (var uow = _unitOfWorkFactory.GetUnitOfWork(false))
            {
                var userRepository = uow.GetRepository<IUserRepository>();
                var result = userRepository.GetUser(userId);

                return Task.FromResult(result);
            }
        }

        public Task<User> FindByNameAsync(string userName)
        {
            using (var uow = _unitOfWorkFactory.GetUnitOfWork(false))
            {
                var userRepository = uow.GetRepository<IUserRepository>();
                var result = userRepository.GetUser(userName);

                return Task.FromResult(result);
            }
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

        public Task AddToRoleAsync(User user, string roleName)
        {
            throw new NotImplementedException();
        }

        public Task RemoveFromRoleAsync(User user, string roleName)
        {
            throw new NotImplementedException();
        }

        public Task<IList<string>> GetRolesAsync(User user)
        {
            return Task.FromResult<IList<string>>(user.UserRoles.Select(o => o.RoleName).ToList());
        }

        public Task<bool> IsInRoleAsync(User user, string roleName)
        {
            return Task.FromResult(user.UserRoles.Any(o => o.RoleName == roleName));
        }
    }
}

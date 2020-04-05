using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Judge.Data;
using Microsoft.AspNet.Identity;

namespace Judge.Application
{
    internal sealed class UserStore : IUserPasswordStore<ApplicationUser, long>, IUserLockoutStore<ApplicationUser, long>, IUserTwoFactorStore<ApplicationUser, long>, IUserRoleStore<ApplicationUser, long>, IUserEmailStore<ApplicationUser, long>
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        public UserStore(IUnitOfWorkFactory unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        public void Dispose()
        {

        }

        public Task CreateAsync(ApplicationUser user)
        {
            using (var uow = _unitOfWorkFactory.GetUnitOfWork())
            {
                var userRepository = uow.UserRepository;
                userRepository.Add(user.User);
                uow.Commit();
            }

            return Task.CompletedTask;
        }

        public Task UpdateAsync(ApplicationUser user)
        {
            using (var uow = _unitOfWorkFactory.GetUnitOfWork())
            {
                var userRepository = uow.UserRepository;
                userRepository.Update(user.User);
                uow.Commit();
            }

            return Task.CompletedTask;
        }

        public Task DeleteAsync(ApplicationUser user)
        {
            using (var uow = _unitOfWorkFactory.GetUnitOfWork())
            {
                var userRepository = uow.UserRepository;
                userRepository.Delete(user.User);
                uow.Commit();
            }

            return Task.CompletedTask;
        }

        public Task<ApplicationUser> FindByIdAsync(long userId)
        {
            using (var uow = _unitOfWorkFactory.GetUnitOfWork())
            {
                var userRepository = uow.UserRepository;
                var result = userRepository.Get(userId);

                return Task.FromResult(new ApplicationUser(result));
            }
        }

        public Task<ApplicationUser> FindByNameAsync(string userName)
        {
            using (var uow = _unitOfWorkFactory.GetUnitOfWork())
            {
                var userRepository = uow.UserRepository;
                var result = userRepository.FindByName(userName);

                return Task.FromResult(result == null ? null : new ApplicationUser(result));
            }
        }

        public Task SetPasswordHashAsync(ApplicationUser user, string passwordHash)
        {
            return Task.FromResult(user.PasswordHash = passwordHash);
        }

        public Task<string> GetPasswordHashAsync(ApplicationUser user)
        {
            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(ApplicationUser user)
        {
            return Task.FromResult(!string.IsNullOrWhiteSpace(user.PasswordHash));
        }

        public Task<DateTimeOffset> GetLockoutEndDateAsync(ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        public Task SetLockoutEndDateAsync(ApplicationUser user, DateTimeOffset lockoutEnd)
        {
            throw new NotImplementedException();
        }

        public Task<int> IncrementAccessFailedCountAsync(ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        public Task ResetAccessFailedCountAsync(ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetAccessFailedCountAsync(ApplicationUser user)
        {
            return Task.FromResult(0);
        }

        public Task<bool> GetLockoutEnabledAsync(ApplicationUser user)
        {
            return Task.FromResult(false);
        }

        public Task SetLockoutEnabledAsync(ApplicationUser user, bool enabled)
        {
            throw new NotImplementedException();
        }

        public Task SetTwoFactorEnabledAsync(ApplicationUser user, bool enabled)
        {
            throw new NotImplementedException();
        }

        public Task<bool> GetTwoFactorEnabledAsync(ApplicationUser user)
        {
            return Task.FromResult(false);
        }

        public Task AddToRoleAsync(ApplicationUser user, string roleName)
        {
            throw new NotImplementedException();
        }

        public Task RemoveFromRoleAsync(ApplicationUser user, string roleName)
        {
            throw new NotImplementedException();
        }

        public Task<IList<string>> GetRolesAsync(ApplicationUser user)
        {
            return Task.FromResult<IList<string>>(user.UserRoles.ToList());
        }

        public Task<bool> IsInRoleAsync(ApplicationUser user, string roleName)
        {
            return Task.FromResult(user.UserRoles.Any(o => o == roleName));
        }

        public Task SetEmailAsync(ApplicationUser user, string email)
        {
            user.Email = email;
            return Task.CompletedTask;
        }

        public Task<string> GetEmailAsync(ApplicationUser user)
        {
            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(ApplicationUser user)
        {
            return Task.FromResult(true);
        }

        public Task SetEmailConfirmedAsync(ApplicationUser user, bool confirmed)
        {
            return Task.CompletedTask;
        }

        public Task<ApplicationUser> FindByEmailAsync(string email)
        {
            using (var uow = _unitOfWorkFactory.GetUnitOfWork())
            {
                var userRepository = uow.UserRepository;
                var result = userRepository.FindByEmail(email);

                return Task.FromResult(result == null ? null : new ApplicationUser(result));
            }
        }
    }
}

using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace Judge.Application
{
    internal sealed class UserTokenProvider : IUserTokenProvider<ApplicationUser,long>
    {
        public Task<string> GenerateAsync(string purpose, UserManager<ApplicationUser, long> manager, ApplicationUser user)
        {
            return Task.FromResult(string.Empty);
        }

        public Task<bool> ValidateAsync(string purpose, string token, UserManager<ApplicationUser, long> manager, ApplicationUser user)
        {
            return Task.FromResult(true);
        }

        public Task NotifyAsync(string token, UserManager<ApplicationUser, long> manager, ApplicationUser user)
        {
            return Task.CompletedTask;
        }

        public Task<bool> IsValidProviderForUserAsync(UserManager<ApplicationUser, long> manager, ApplicationUser user)
        {
            return Task.FromResult(true);
        }
    }
}

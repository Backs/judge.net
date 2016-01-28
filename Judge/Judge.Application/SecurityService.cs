using Judge.Application.Interfaces;
using Judge.Model.Entities;
using Microsoft.AspNet.Identity;

namespace Judge.Application
{
    internal sealed class SecurityService : ISecurityService
    {
        private readonly UserManager<User, long> _userManager;
        public SecurityService(UserManager<User, long> userManager)
        {
            _userManager = userManager;
        }
    }
}

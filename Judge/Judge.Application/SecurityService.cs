using Judge.Application.Interfaces;
using Judge.Application.ViewModels.Account;
using Judge.Model.Entities;
using Microsoft.AspNet.Identity.Owin;

namespace Judge.Application
{
    internal sealed class SecurityService : ISecurityService
    {
        private readonly SignInManager<User, long> _signInManager;
        public SecurityService(SignInManager<User, long> signInManager)
        {
            _signInManager = signInManager;
        }

        public SignInStatus SignIn(string userName, string password, bool isPersistent)
        {
            var status = _signInManager.PasswordSignIn(userName, password, isPersistent, shouldLockout: false);
            return status;
        }

        public void Register(RegisterViewModel model)
        {
            _signInManager.UserManager.CreateAsync(new User { UserName = model.Email }, model.Password).Wait();
        }
    }
}

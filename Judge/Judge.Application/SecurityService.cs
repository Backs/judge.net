using Judge.Application.Interfaces;
using Judge.Application.ViewModels.Account;
using Judge.Model.Entities;
using Microsoft.AspNet.Identity;
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

        public SignInStatus SignIn(string email, string password, bool isPersistent)
        {
            var status = _signInManager.PasswordSignIn(email, password, isPersistent, shouldLockout: false);
            return status;
        }

        public void Register(RegisterViewModel model)
        {
            _signInManager.UserManager.CreateAsync(new User { UserName = model.UserName, Email = model.Email }, model.Password).Wait();
        }

        public void SignOut()
        {
            _signInManager.AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie,
                DefaultAuthenticationTypes.TwoFactorCookie);
        }

        public bool UserExists(string email)
        {
            return _signInManager.UserManager.FindByName(email) != null;
        }
    }
}

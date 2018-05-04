using System;
using Judge.Application.Interfaces;
using Judge.Application.ViewModels.Account;
using Judge.Model.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace Judge.Application
{
    internal sealed class SecurityService : ISecurityService
    {
        private readonly Lazy<SignInManager<User, long>> _signInManager;

        private SignInManager<User, long> SignInManager => _signInManager.Value;

        public SecurityService(IOwinContextAccessor owinContextAccessor, UserManager<User, long> userManager)
        {
            _signInManager = new Lazy<SignInManager<User, long>>(() =>
            {
                var manager = new SignInManager<User, long>(userManager, owinContextAccessor.CurrentContext.Authentication);

                manager.UserManager.UserValidator = new UserValidator<User, long>(manager.UserManager)
                {
                    AllowOnlyAlphanumericUserNames = false
                };
                return manager;
            });
        }

        public SignInStatus SignIn(string email, string password, bool isPersistent)
        {
            var status = SignInManager.PasswordSignIn(email, password, isPersistent, shouldLockout: false);
            return status;
        }

        public void Register(RegisterViewModel model)
        {
            SignInManager.UserManager.CreateAsync(new User { UserName = model.UserName, Email = model.Email }, model.Password).Wait();
        }

        public void SignOut()
        {
            SignInManager.AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie,
                DefaultAuthenticationTypes.TwoFactorCookie);
        }

        public bool UserExists(string email)
        {
            return SignInManager.UserManager.FindByName(email) != null;
        }
    }
}

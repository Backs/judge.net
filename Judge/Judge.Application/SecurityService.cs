using System;
using Judge.Application.Interfaces;
using Judge.Application.ViewModels.Account;
using Judge.Application.ViewModels.Admin.Users;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace Judge.Application
{
    internal sealed class SecurityService : ISecurityService
    {
        private readonly Lazy<SignInManager<ApplicationUser, long>> _signInManager;

        private SignInManager<ApplicationUser, long> SignInManager => _signInManager.Value;

        public SecurityService(IOwinContextAccessor owinContextAccessor, UserManager<ApplicationUser, long> userManager)
        {
            _signInManager = new Lazy<SignInManager<ApplicationUser, long>>(() =>
            {
                var manager = new SignInManager<ApplicationUser, long>(userManager, owinContextAccessor.CurrentContext.Authentication);

                manager.UserManager.UserValidator = new UserValidator<ApplicationUser, long>(manager.UserManager)
                {
                    AllowOnlyAlphanumericUserNames = false,
                    RequireUniqueEmail = true
                };
                manager.UserManager.UserTokenProvider = new UserTokenProvider();
                return manager;
            });
        }

        public SignInStatus SignIn(string email, string password, bool isPersistent)
        {
            var signedUser = SignInManager.UserManager.FindByEmail(email);

            if (signedUser == null)
            {
                return SignInStatus.Failure;
            }

            var status = SignInManager.PasswordSignIn(signedUser.UserName, password, isPersistent, shouldLockout: false);
            return status;
        }

        public RegistrationResult Register(RegisterViewModel model)
        {
            var identityResult = SignInManager.UserManager.CreateAsync(new ApplicationUser { UserName = model.UserName, Email = model.Email }, model.Password).Result;

            return new RegistrationResult
            {
                Succeeded = identityResult.Succeeded,
                Errors = identityResult.Errors
            };
        }

        public void SignOut()
        {
            SignInManager.AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie,
                DefaultAuthenticationTypes.TwoFactorCookie);
        }

        public void UpdateUser(UserEditViewModel model)
        {
            var user = SignInManager.UserManager.FindById(model.Id);

            user.Email = model.Email;
            user.UserName = model.UserName;

            SignInManager.UserManager.Update(user);

            if (!string.IsNullOrWhiteSpace(model.NewPassword))
            {
                SignInManager.UserManager.ResetPassword(model.Id, null, model.NewPassword);
            }
        }
    }
}

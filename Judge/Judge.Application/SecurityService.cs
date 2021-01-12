namespace Judge.Application
{
    using System;
    using Judge.Application.Interfaces;
    using Judge.Application.ViewModels.Account;
    using Judge.Application.ViewModels.Admin.Users;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.Owin;

    internal sealed class SecurityService : ISecurityService
    {
        private readonly Lazy<SignInManager<ApplicationUser, long>> signInManager;

        private SignInManager<ApplicationUser, long> SignInManager => this.signInManager.Value;

        public SecurityService(IOwinContextAccessor owinContextAccessor, UserManager<ApplicationUser, long> userManager)
        {
            this.signInManager = new Lazy<SignInManager<ApplicationUser, long>>(() =>
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
            var signedUser = this.SignInManager.UserManager.FindByEmail(email);

            if (signedUser == null)
            {
                return SignInStatus.Failure;
            }

            var status = this.SignInManager.PasswordSignIn(signedUser.UserName, password, isPersistent, shouldLockout: false);
            return status;
        }

        public RegistrationResult Register(RegisterViewModel model)
        {
            var identityResult = this.SignInManager.UserManager.CreateAsync(new ApplicationUser { UserName = model.UserName, Email = model.Email }, model.Password).Result;

            return new RegistrationResult
            {
                Succeeded = identityResult.Succeeded,
                Errors = identityResult.Errors
            };
        }

        public void SignOut()
        {
            this.SignInManager.AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie,
                DefaultAuthenticationTypes.TwoFactorCookie);
        }

        public void UpdateUser(UserEditViewModel model)
        {
            var user = this.SignInManager.UserManager.FindById(model.Id);

            user.Email = model.Email;
            user.UserName = model.UserName;

            this.SignInManager.UserManager.Update(user);

            if (!string.IsNullOrWhiteSpace(model.NewPassword))
            {
                this.SignInManager.UserManager.ResetPassword(model.Id, null, model.NewPassword);
            }
        }
    }
}

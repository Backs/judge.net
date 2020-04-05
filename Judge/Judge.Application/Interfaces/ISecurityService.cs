using Judge.Application.ViewModels.Account;
using Judge.Application.ViewModels.Admin.Users;
using Microsoft.AspNet.Identity.Owin;

namespace Judge.Application.Interfaces
{
    public interface ISecurityService
    {
        SignInStatus SignIn(string email, string password, bool isPersistent);

        void SignOut();

        RegistrationResult Register(RegisterViewModel model);

        void UpdateUser(UserEditViewModel model);
    }
}

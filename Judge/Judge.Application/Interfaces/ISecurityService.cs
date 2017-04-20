using Judge.Application.ViewModels.Account;
using Microsoft.AspNet.Identity.Owin;

namespace Judge.Application.Interfaces
{
    public interface ISecurityService
    {
        SignInStatus SignIn(string email, string password, bool isPersistent);
        void SignOut();
        void Register(RegisterViewModel model);
        bool UserExists(string email);
    }
}

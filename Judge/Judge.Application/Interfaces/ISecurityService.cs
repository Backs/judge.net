using Microsoft.AspNet.Identity.Owin;

namespace Judge.Application.Interfaces
{
    public interface ISecurityService
    {
        SignInStatus SignIn(string userName, string password, bool isPersistent);
    }
}

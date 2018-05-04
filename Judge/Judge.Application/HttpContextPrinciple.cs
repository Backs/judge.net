using System.Security.Principal;
using System.Web;

namespace Judge.Application
{
    public class HttpContextPrinciple : IPrincipal
    {
        public IIdentity Identity => HttpContext.Current.User.Identity;
        public bool IsInRole(string role) => HttpContext.Current.User.IsInRole(role);
    }
}

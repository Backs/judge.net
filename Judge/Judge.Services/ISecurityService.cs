using System.Threading.Tasks;
using Judge.Web.Client.Login;

namespace Judge.Services;

public interface ISecurityService
{
    Task<Authentication> AuthenticateAsync(Login login);
}
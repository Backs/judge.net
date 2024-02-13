using System.Threading.Tasks;
using Judge.Services.Model;
using Judge.Web.Client.Login;
using Judge.Web.Client.Users;

namespace Judge.Services;

public interface ISecurityService
{
    Task<Authentication> AuthenticateAsync(Login login);
    Task<CreateUserResponse> CreateUserAsync(CreateUser user);
}
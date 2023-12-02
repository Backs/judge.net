using System.Threading.Tasks;
using Judge.Model.Entities;
using Judge.Web.Client.Login;

namespace Judge.Services
{
    public interface ISecurityService
    {
        Task<(AuthenticateResult, User?)> AuthenticateAsync(Login login);
    }
}
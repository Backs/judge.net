using System.Threading.Tasks;
using Judge.Web.Client.Users;

namespace Judge.Services;

public interface IUsersService
{
    Task<CurrentUser?> GetUserAsync(long id);
    Task<UsersList> SearchAsync(UsersQuery query);
}
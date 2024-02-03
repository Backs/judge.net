using System.Threading.Tasks;
using Judge.Web.Client.Users;

namespace Judge.Services;

public interface IUsersService
{
    Task<User?> GetUserAsync(long id);
    Task<UsersList> SearchAsync(UsersQuery query);
}
using System.Threading.Tasks;
using Judge.Data;
using Judge.Web.Client.Users;

namespace Judge.Services;

internal sealed class UsersService : IUsersService
{
    private readonly IUnitOfWorkFactory unitOfWorkFactory;

    public UsersService(IUnitOfWorkFactory unitOfWorkFactory)
    {
        this.unitOfWorkFactory = unitOfWorkFactory;
    }

    public async Task<User?> GetUserAsync(long id)
    {
        await using var unitOfWork = this.unitOfWorkFactory.GetUnitOfWork();
        var user = await unitOfWork.UserRepository.GetAsync(id);

        if (user == null)
            return null;

        return new User
        {
            Id = user.Id,
            Email = user.Email,
            Login = user.UserName
        };
    }
}
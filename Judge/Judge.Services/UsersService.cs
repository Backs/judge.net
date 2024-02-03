using System.Linq;
using System.Threading.Tasks;
using Judge.Data;
using Judge.Model.Account;
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
        var user = await unitOfWork.Users.GetAsync(id);

        if (user == null)
            return null;

        return new User
        {
            Id = user.Id,
            Email = user.Email,
            Login = user.UserName
        };
    }

    public async Task<UsersList> SearchAsync(UsersQuery query)
    {
        await using var unitOfWork = this.unitOfWorkFactory.GetUnitOfWork();
        var specification = new UsersSpecification(query.Name);
        var user = await unitOfWork.Users.SearchAsync(specification, query.Skip, query.Take);
        var totalCount = await unitOfWork.Users.CountAsync(specification);

        return new UsersList
        {
            TotalCount = totalCount,
            Items = user.Select(o => new User
            {
                Email = o.Email,
                Login = o.UserName,
                Id = o.Id
            }).ToArray()
        };
    }
}
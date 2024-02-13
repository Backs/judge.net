using System.Linq;
using System.Threading.Tasks;
using Judge.Data;
using Judge.Services.Model;
using Judge.Web.Client.Login;
using Judge.Web.Client.Users;
using Microsoft.AspNetCore.Identity;
using User = Judge.Model.Entities.User;

namespace Judge.Services;

internal sealed class SecurityService : ISecurityService
{
    private readonly IUnitOfWorkFactory unitOfWorkFactory;
    private readonly IPasswordHasher<User> passwordHasher;

    public SecurityService(IUnitOfWorkFactory unitOfWorkFactory, IPasswordHasher<User> passwordHasher)
    {
        this.unitOfWorkFactory = unitOfWorkFactory;
        this.passwordHasher = passwordHasher;
    }

    public async Task<Authentication> AuthenticateAsync(Login login)
    {
        await using var unitOfWork = this.unitOfWorkFactory.GetUnitOfWork();
        var user = await unitOfWork.Users.FindByEmailAsync(login.Email);

        if (user == null)
        {
            return Authentication.UserNotFound();
        }

        var result = this.passwordHasher.VerifyHashedPassword(user, user.PasswordHash, login.Password);

        if (result == PasswordVerificationResult.Failed)
        {
            return Authentication.PasswordVerificationFailed();
        }

        return Authentication.Success(
            new Web.Client.Users.User { Email = user.Email, Login = user.UserName, Id = user.Id },
            user.UserRoles.Select(o => o.RoleName).ToArray());
    }

    public async Task<CreateUserResponse> CreateUserAsync(CreateUser user)
    {
        await using var unitOfWork = this.unitOfWorkFactory.GetUnitOfWork();
        var existedUser = await unitOfWork.Users.FindByEmailAsync(user.Email) ??
                          await unitOfWork.Users.FindByLoginASync(user.Login);

        if (existedUser != null)
        {
            return new CreateUserResponse(CreateUserResult.Conflict, null);
        }

        var newUser = new User
        {
            Email = user.Email,
            UserName = user.Login
        };

        newUser.PasswordHash = this.passwordHasher.HashPassword(newUser, user.Password);

        unitOfWork.Users.Add(newUser);
        await unitOfWork.CommitAsync();

        return new CreateUserResponse(CreateUserResult.Success,
            new Web.Client.Users.User { Email = user.Email, Login = user.Login, Id = newUser.Id });
    }
}
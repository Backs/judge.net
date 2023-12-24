using System.Linq;
using System.Threading.Tasks;
using Judge.Data;
using Judge.Model.Entities;
using Judge.Web.Client.Login;
using Microsoft.AspNetCore.Identity;

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
        var user = await unitOfWork.UserRepository.FindByEmailAsync(login.Email);

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
            new Web.Client.Users.User {Email = user.Email, Login = user.UserName, Id = user.Id},
            user.UserRoles.Select(o => o.RoleName).ToArray());
    }
}
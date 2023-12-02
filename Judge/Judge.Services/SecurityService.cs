using System.Threading.Tasks;
using Judge.Data;
using Judge.Model.Entities;
using Judge.Web.Client.Login;
using Microsoft.AspNet.Identity;

namespace Judge.Services
{
    internal sealed class SecurityService : ISecurityService
    {
        private readonly IUnitOfWorkFactory unitOfWorkFactory;
        private readonly IPasswordHasher passwordHasher;

        public SecurityService(IUnitOfWorkFactory unitOfWorkFactory, IPasswordHasher passwordHasher)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
            this.passwordHasher = passwordHasher;
        }

        public async Task<(AuthenticateResult, User?)> AuthenticateAsync(Login login)
        {
            await using var unitOfWork = this.unitOfWorkFactory.GetUnitOfWork();
            var user = await unitOfWork.UserRepository.FindByEmailAsync(login.Email);

            if (user == null)
            {
                return (AuthenticateResult.UserNotFound, null);
            }

            var result = this.passwordHasher.VerifyHashedPassword(user.PasswordHash, login.Password);

            if (result == PasswordVerificationResult.Failed)
            {
                return (AuthenticateResult.PasswordVerificationFailed, null);
            }

            return (AuthenticateResult.Success, user);
        }
    }
}
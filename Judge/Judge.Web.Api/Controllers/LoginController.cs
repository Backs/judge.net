using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Judge.Services;
using Judge.Web.Client.Login;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Judge.Web.Api.Controllers
{
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ISecurityService securityService;

        public LoginController(ISecurityService securityService)
        {
            this.securityService = securityService;
        }

        [HttpPost("token")]
        public async Task<IActionResult> CreateToken([FromBody] Login login)
        {
            var (authenticateResult, user) = await this.securityService.AuthenticateAsync(login);

            if (authenticateResult == AuthenticateResult.UserNotFound)
                return this.NotFound();

            if (authenticateResult == AuthenticateResult.PasswordVerificationFailed)
                return this.Unauthorized();

            if (authenticateResult == AuthenticateResult.Success)
            {
                var issuer = "issuer";
                var audience = "issuer";
                var key = Encoding.ASCII.GetBytes("kjsnakgalksaldjfncskldjnjbfakldbdfjbvalskdbasjvbahvb");
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                        new Claim("Id", user!.Id.ToString()),
                        new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                        new Claim(JwtRegisteredClaimNames.Email, user.Email)
                    }),
                    Expires = DateTime.UtcNow.AddMinutes(5),
                    Issuer = issuer,
                    Audience = audience,
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                        SecurityAlgorithms.HmacSha512Signature)
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var jwtToken = tokenHandler.WriteToken(token);
                return this.Ok(new TokenResult {Token = jwtToken});
            }

            return this.BadRequest();
        }
    }
}
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Judge.Services;
using Judge.Web.Client.Login;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Judge.Web.Api.Controllers;

[Route("login")]
[ApiController]
public class LoginController : ControllerBase
{
    private readonly ISecurityService securityService;
    private readonly IConfiguration configuration;

    public LoginController(ISecurityService securityService, IConfiguration configuration)
    {
        this.securityService = securityService;
        this.configuration = configuration;
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
            var key = Encoding.ASCII.GetBytes(this.configuration["AppSettings:SecurityKey"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id", user!.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email)
                }),
                Expires = DateTime.UtcNow.Add(this.configuration.GetValue<TimeSpan>("AppSettings:Expires")),
                Issuer = this.configuration["AppSettings:Issuer"],
                Audience = this.configuration["AppSettings:Audience"],
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
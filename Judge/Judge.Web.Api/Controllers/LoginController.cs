using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
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
        var (authenticationResult, user, roles) = await this.securityService.AuthenticateAsync(login);

        if (authenticationResult == AuthenticationResult.UserNotFound)
            return this.NotFound();

        if (authenticationResult == AuthenticationResult.PasswordVerificationFailed)
            return this.Unauthorized();

        if (authenticationResult == AuthenticationResult.Success)
        {
            var key = Encoding.ASCII.GetBytes(this.configuration["AppSettings:SecurityKey"]);
            var claims = new List<Claim>(4)
            {
                new("Id", user!.Id.ToString()),
                new(JwtRegisteredClaimNames.Sub, user.Login),
                new(JwtRegisteredClaimNames.Email, user.Email)
            };

            claims.AddRange(roles.Select(o => new Claim(ClaimTypes.Role, o)));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.Add(this.configuration.GetValue<TimeSpan>("AppSettings:Expires")),
                Issuer = this.configuration["AppSettings:Issuer"],
                Audience = this.configuration["AppSettings:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha512Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = tokenHandler.WriteToken(token);
            return this.Ok(new TokenResult { Token = jwtToken });
        }

        return this.BadRequest();
    }
}
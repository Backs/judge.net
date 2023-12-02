using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Judge.Web.Client.Login;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Judge.Web.Api.Controllers
{
    [ApiController]
    public class LoginController : ControllerBase
    {
        [HttpPost("token")]
        public async Task<IActionResult> CreateToken([FromBody] CreateToken createToken)
        {
            var issuer = "issuer";
            var audience = "issuer";
            var key = Encoding.ASCII.GetBytes("kjsnakgalksaldjfncskldjnjbfakldbdfjbvalskdbasjvbahvb");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id", Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Sub, "user"),
                    new Claim(JwtRegisteredClaimNames.Email, "email"),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
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
    }
}
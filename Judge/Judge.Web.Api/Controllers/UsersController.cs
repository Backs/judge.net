using System.Security.Claims;
using System.Threading.Tasks;
using Judge.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Judge.Web.Api.Controllers;

[Authorize]
[Route("users")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUsersService usersService;

    public UsersController(IUsersService usersService)
    {
        this.usersService = usersService;
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetCurrentUser()
    {
        var identity = this.User.Identity as ClaimsIdentity;
        var id = long.Parse(identity!.FindFirst("id")!.Value);
        var user = await this.usersService.GetUserAsync(id);
        if (user == null)
            return this.NotFound();

        return this.Ok(user);
    }
}
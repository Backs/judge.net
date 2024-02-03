using System.Threading.Tasks;
using Judge.Services;
using Judge.Web.Api.Extensions;
using Judge.Web.Client.Users;
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
        var user = await this.usersService.GetUserAsync(this.User.GetUserId());
        if (user == null)
            return this.NotFound();

        return this.Ok(user);
    }

    [HttpGet]
    public async Task<IActionResult> Search([FromQuery] UsersQuery? query)
    {
        query ??= new UsersQuery();

        var result = await this.usersService.SearchAsync(query);

        return this.Ok(result);
    }
}
using System;
using System.Threading.Tasks;
using Judge.Services;
using Judge.Services.Model;
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
    private readonly ISecurityService securityService;

    public UsersController(IUsersService usersService, ISecurityService securityService)
    {
        this.usersService = usersService;
        this.securityService = securityService;
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

    [HttpPut]
    public async Task<IActionResult> Create([FromBody] CreateUser user)
    {
        var response = await this.securityService.CreateUserAsync(user);

        switch (response.Result)
        {
            case CreateUserResult.Conflict:
                return this.Conflict();
            case CreateUserResult.Success:
                return this.Ok(response.User);
            case CreateUserResult.Error:
                return this.BadRequest();
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
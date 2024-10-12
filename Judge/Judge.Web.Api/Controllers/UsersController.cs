using System;
using System.Threading.Tasks;
using Judge.Services;
using Judge.Services.Model;
using Judge.Web.Client.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Judge.Web.Api.Controllers;

/// <summary>
/// Users 
/// </summary>
[Route("api/users")]
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

    /// <summary>
    /// Get current user information
    /// </summary>
    [Authorize]
    [HttpGet("me")]
    [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCurrentUser()
    {
        var user = await this.usersService.GetUserAsync(this.User.GetUserId());
        if (user == null)
            return this.NotFound();

        return this.Ok(user);
    }

    /// <summary>
    /// Search users
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(UsersList), StatusCodes.Status200OK)]
    public async Task<IActionResult> Search([FromQuery] UsersQuery? query)
    {
        query ??= new UsersQuery();

        var result = await this.usersService.SearchAsync(query);

        return this.Ok(result);
    }

    /// <summary>
    /// Create new user
    /// </summary>
    /// <param name="user"><inheritdoc cref="Judge.Web.Client.Users.User"/></param>
    [HttpPut]
    [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
    public async Task<IActionResult> Create([FromBody] CreateUser user)
    {
        var response = await this.securityService.CreateUserAsync(user);

        return response.Result switch
        {
            CreateUserResult.Conflict => this.Conflict(),
            CreateUserResult.Success => this.Ok(response.User),
            CreateUserResult.Error => this.BadRequest(),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}
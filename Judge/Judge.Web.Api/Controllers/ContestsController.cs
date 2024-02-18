using System.Threading.Tasks;
using Judge.Services;
using Judge.Web.Api.Authorization;
using Judge.Web.Api.Extensions;
using Judge.Web.Client.Contests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Judge.Web.Api.Controllers;

/// <summary>
/// Contests
/// </summary>
[Route("api/contests")]
[ApiController]
public class ContestsController : ControllerBase
{
    private readonly IContestsService contestsService;

    public ContestsController(IContestsService contestsService)
    {
        this.contestsService = contestsService;
    }

    /// <summary>
    /// Search contests
    /// </summary>
    /// <param name="query">Search query</param>
    [HttpGet]
    [ProducesResponseType(typeof(ContestsInfoList), StatusCodes.Status200OK)]
    public async Task<IActionResult> Search([FromQuery] ContestsQuery? query)
    {
        query ??= new ContestsQuery();

        var result = await this.contestsService.SearchAsync(query);

        return this.Ok(result);
    }

    /// <summary>
    /// Get contest by id
    /// </summary>
    /// <param name="contestId">Contest id</param>
    [HttpGet("{contestId:int}")]
    [ProducesResponseType(typeof(Contest), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get([FromRoute] int contestId)
    {
        var contest = await this.contestsService.GetAsync(contestId, this.User.TryGetUserId());

        if (contest == null)
            return this.NotFound();

        return this.Ok(contest);
    }

    /// <summary>
    /// Create or update contest information
    /// </summary>
    /// <param name="contest">Contest information</param>
    [Authorize(AuthorizationPolicies.AdminPolicy)]
    [HttpPut]
    [ProducesResponseType(typeof(EditContest), StatusCodes.Status200OK)]
    public async Task<IActionResult> Save([FromBody] EditContest contest)
    {
        var result = await this.contestsService.SaveAsync(contest);
        if (result == null)
            return this.NotFound();

        return this.Ok(result);
    }

    /// <summary>
    /// Get contest results
    /// </summary>
    /// <param name="contestId">Contest id</param>
    [HttpGet("{contestId:int}/results")]
    [ProducesResponseType(typeof(ContestResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetResults([FromRoute] int contestId)
    {
        var contest = await this.contestsService.GetResultAsync(contestId);

        if (contest == null)
            return this.NotFound();

        return this.Ok(contest);
    }
}
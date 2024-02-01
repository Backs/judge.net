using System.Threading.Tasks;
using Judge.Services;
using Judge.Web.Api.Authorization;
using Judge.Web.Api.Extensions;
using Judge.Web.Client.Contests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Judge.Web.Api.Controllers;

[Route("contests")]
[ApiController]
public class ContestsController : ControllerBase
{
    private readonly IContestsService contestsService;

    public ContestsController(IContestsService contestsService)
    {
        this.contestsService = contestsService;
    }

    [HttpGet]
    public async Task<IActionResult> Search([FromQuery] ContestsQuery? query)
    {
        query ??= new ContestsQuery();

        var result = await this.contestsService.SearchAsync(query);

        return this.Ok(result);
    }

    [HttpGet("{contestId:int}")]
    public async Task<IActionResult> Get([FromRoute] int contestId)
    {
        var contest = await this.contestsService.GetAsync(contestId, this.User.TryGetUserId());

        if (contest == null)
            return this.NotFound();

        return this.Ok(contest);
    }

    [Authorize(AuthorizationPolicies.AdminPolicy)]
    [HttpPut]
    public async Task<IActionResult> Save([FromBody] EditContest contest)
    {
        var result = await this.contestsService.SaveAsync(contest);
        if (result == null)
            return this.NotFound();

        return this.Ok(result);
    }

    [HttpGet("{contestId:int}/results")]
    public async Task<IActionResult> GetResults([FromRoute] int contestId)
    {
        var contest = await this.contestsService.GetResultAsync(contestId);

        if (contest == null)
            return this.NotFound();

        return this.Ok(contest);
    }
}
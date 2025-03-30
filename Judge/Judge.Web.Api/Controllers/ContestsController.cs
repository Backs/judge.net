using System.Threading.Tasks;
using Judge.Services;
using Judge.Web.Api.Authorization;
using Judge.Web.Client.Contests;
using Judge.Web.Client.Problems;
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

        var openedOnly = !this.User.IsAdmin();
        var result = await this.contestsService.SearchAsync(query, openedOnly);

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

        if (!contest.IsOpened && !this.User.IsAdmin())
            return this.NotFound();

        return this.Ok(contest);
    }

    /// <summary>
    /// Get contest problem
    /// </summary>
    /// <param name="contestId">Contest id</param>
    /// <param name="label">Problem label</param>
    [HttpGet("{contestId:int}/{label}")]
    [ProducesResponseType(typeof(Problem), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProblem([FromRoute] int contestId, [FromRoute] string label)
    {
        var problem = await this.contestsService.GetProblemAsync(contestId, label, this.User.TryGetUserId());

        if (problem == null)
            return this.NotFound();

        return this.Ok(problem);
    }

    /// <summary>
    /// Get editable contest information
    /// </summary>
    /// <param name="contestId">Contest id</param>
    [Authorize(AuthorizationPolicies.AdminPolicy)]
    [HttpGet("{contestId:int}/editable")]
    [ProducesResponseType(typeof(EditContest), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetEditable([FromRoute] int contestId)
    {
        var contest = await this.contestsService.GetEditableAsync(contestId);

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

        if (!contest.IsOpened && !this.User.IsAdmin())
            return this.NotFound();

        return this.Ok(contest);
    }

    /// <summary>
    /// Get contest analysis
    /// </summary>
    /// <param name="contestId">Contest id</param>
    [HttpGet("{contestId:int}/analysis")]
    [ProducesResponseType(typeof(ContestAnalysisInfo), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAnalysis([FromRoute] int contestId)
    {
        var contest = await this.contestsService.GetAnalysisAsync(contestId);

        if (contest == null)
            return this.NotFound();

        if (contest.Status != ContestStatus.Completed && !this.User.IsAdmin())
            return this.NotFound();

        return this.Ok(contest);
    }
}
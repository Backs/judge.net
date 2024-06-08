using System.Threading.Tasks;
using Judge.Services;
using Judge.Web.Api.Authorization;
using Judge.Web.Api.Extensions;
using Judge.Web.Client.Problems;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Judge.Web.Api.Controllers;

/// <summary>
/// Problems
/// </summary>
[Route("api/problems")]
[ApiController]
public class ProblemsController : ControllerBase
{
    private readonly IProblemsService problemsService;

    public ProblemsController(IProblemsService problemsService)
    {
        this.problemsService = problemsService;
    }

    /// <summary>
    /// Search problems
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ProblemsList), StatusCodes.Status200OK)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Search([FromQuery] ProblemsQuery? query)
    {
        query ??= new ProblemsQuery();

        var result = await this.problemsService.SearchAsync(this.User.TryGetUserId(), query);

        return this.Ok(result);
    }

    /// <summary>
    /// Get problem by id
    /// </summary>
    /// <param name="id">Problem id</param>
    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(Problem), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get([FromRoute] long id)
    {
        var result = await this.problemsService.GetAsync(id);

        if (result == null)
            return this.NotFound();

        return this.Ok(result);
    }

    /// <summary>
    /// Crate or edit new problem
    /// </summary>
    /// <param name="problem"></param>
    [Authorize(AuthorizationPolicies.AdminPolicy)]
    [HttpPut]
    [ProducesResponseType(typeof(EditProblem), StatusCodes.Status200OK)]
    public async Task<IActionResult> Save([FromBody] EditProblem problem)
    {
        var result = await this.problemsService.SaveAsync(problem);

        if (result == null)
            return this.NotFound();

        return this.Ok(result);
    }
}
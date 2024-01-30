using System.Threading.Tasks;
using Judge.Services;
using Judge.Web.Api.Authorization;
using Judge.Web.Api.Extensions;
using Judge.Web.Client.Problems;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Judge.Web.Api.Controllers;

[Route("problems")]
[ApiController]
public class ProblemsController : ControllerBase
{
    private readonly IProblemsService problemsService;

    public ProblemsController(IProblemsService problemsService)
    {
        this.problemsService = problemsService;
    }

    [HttpGet]
    public async Task<IActionResult> Search([FromQuery] ProblemsQuery? query)
    {
        query ??= new ProblemsQuery();

        var result = await this.problemsService.SearchAsync(this.User.TryGetUserId(), query);

        return this.Ok(result);
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> Get([FromRoute] long id)
    {
        var result = await this.problemsService.GetAsync(id);

        if (result == null)
            return this.NotFound();

        return this.Ok(result);
    }

    [Authorize(AuthorizationPolicies.AdminPolicy)]
    [HttpPut]
    public async Task<IActionResult> Save([FromBody] EditProblem problem)
    {
        var result = await this.problemsService.SaveAsync(problem);

        if (result == null)
            return this.NotFound();

        return this.Ok(result);
    }
}
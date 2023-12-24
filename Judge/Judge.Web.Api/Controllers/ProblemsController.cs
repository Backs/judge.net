using System.Threading.Tasks;
using Judge.Services;
using Judge.Web.Api.Extensions;
using Judge.Web.Client.Problems;
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
    public async Task<IActionResult> GetProblems([FromQuery] ProblemsQuery? query)
    {
        query ??= new ProblemsQuery();

        var result = await this.problemsService.SearchAsync(this.User.TryGetUserId(), query);

        return this.Ok(result);
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetProblem([FromRoute] long id)
    {
        var result = await this.problemsService.GetAsync(id);

        if (result == null)
            return this.NotFound();

        return this.Ok(result);
    }
}
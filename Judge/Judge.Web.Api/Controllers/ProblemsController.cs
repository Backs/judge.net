using System.Threading.Tasks;
using Judge.Services;
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

        var result = await this.problemsService.GetProblemsAsync(query);

        return this.Ok(result);
    }
}
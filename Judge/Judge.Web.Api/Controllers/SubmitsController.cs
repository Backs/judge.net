using System.Threading.Tasks;
using Judge.Services;
using Judge.Web.Client.Submits;
using Microsoft.AspNetCore.Mvc;

namespace Judge.Web.Api.Controllers;

[Route("")]
[ApiController]
public class SubmitsController : ControllerBase
{
    private readonly ISubmitsService submitsService;

    public SubmitsController(ISubmitsService submitsService)
    {
        this.submitsService = submitsService;
    }

    [HttpGet("submits")]
    public async Task<IActionResult> GetSubmitQueue([FromQuery] SubmitsQuery? query)
    {
        query ??= new SubmitsQuery();

        var result = await this.submitsService.SearchAsync(query);

        return this.Ok(result);
    }
}
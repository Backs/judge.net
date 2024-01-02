using System;
using System.Threading.Tasks;
using Judge.Services;
using Judge.Web.Api.Extensions;
using Judge.Web.Client.Submits;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Judge.Web.Api.Controllers;

[Route("submits")]
[ApiController]
public class SubmitsController : ControllerBase
{
    private readonly ISubmitsService submitsService;

    public SubmitsController(ISubmitsService submitsService)
    {
        this.submitsService = submitsService;
    }

    [HttpGet]
    public async Task<IActionResult> GetSubmitQueue([FromQuery] SubmitsQuery? query)
    {
        query ??= new SubmitsQuery();

        var result = await this.submitsService.SearchAsync(query);

        return this.Ok(result);
    }

    [Authorize]
    [HttpPut]
    public async Task<IActionResult> SubmitSolution([FromForm] SubmitSolution submitSolution)
    {
        var userId = this.User.GetUserId();
        var userInfo = new SubmitUserInfo(userId, this.Request.Host.ToString());
        try
        {
            await this.submitsService.SaveAsync(submitSolution, userInfo);
        }
        catch (InvalidOperationException ex)
        {
            return this.BadRequest(ex.Message);
        }

        return this.Ok();
    }
}
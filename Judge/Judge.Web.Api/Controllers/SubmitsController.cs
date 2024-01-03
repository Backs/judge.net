using System;
using System.Threading.Tasks;
using Judge.Services;
using Judge.Services.Model;
using Judge.Web.Api.Extensions;
using Judge.Web.Client.Submits;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SubmitsQuery = Judge.Web.Client.Submits.SubmitsQuery;

namespace Judge.Web.Api.Controllers;

[ApiController]
public class SubmitsController : ControllerBase
{
    private readonly ISubmitsService submitsService;

    public SubmitsController(ISubmitsService submitsService)
    {
        this.submitsService = submitsService;
    }

    [HttpGet("submits")]
    public async Task<IActionResult> GetSubmits([FromQuery] SubmitsQuery? query)
    {
        query ??= new SubmitsQuery();

        var submitsQuery = new Services.Model.SubmitsQuery
        {
            ProblemId = query.ProblemId,
            Skip = query.Skip,
            Take = query.Take,
            ContestId = query.ContestId,
            TaskLabel = query.TaskLabel
        };
        var result = await this.submitsService.SearchAsync(submitsQuery);

        return this.Ok(result);
    }

    [Authorize]
    [HttpGet("problems/{problemId:long}/submits")]
    public async Task<IActionResult> GetTaskSubmits(
        [FromRoute] long problemId,
        [FromQuery] int skip = 0,
        [FromQuery] int take = 20)
    {
        var query = new Services.Model.SubmitsQuery
        {
            Type = SubmitsType.Problem,
            ProblemId = problemId,
            UserId = this.User.GetUserId(),
            Skip = skip,
            Take = take
        };

        var result = await this.submitsService.SearchAsync(query);

        return this.Ok(result);
    }

    [Authorize]
    [HttpPut("submits")]
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
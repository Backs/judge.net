using System;
using System.Threading.Tasks;
using Judge.Services;
using Judge.Services.Model;
using Judge.Web.Api.Authorization;
using Judge.Web.Client.Submits;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SubmitsQuery = Judge.Web.Client.Submits.SubmitsQuery;

namespace Judge.Web.Api.Controllers;

/// <summary>
/// Submits
/// </summary>
[Route("api")]
[ApiController]
public class SubmitsController : ControllerBase
{
    private readonly ISubmitsService submitsService;

    public SubmitsController(ISubmitsService submitsService)
    {
        this.submitsService = submitsService;
    }

    /// <summary>
    /// Search submits
    /// </summary>
    [HttpGet("submits")]
    [ProducesResponseType(typeof(SubmitResultsList), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSubmits([FromQuery] SubmitsQuery? query)
    {
        query ??= new SubmitsQuery();

        var submitsQuery = new Services.Model.SubmitsQuery
        {
            ProblemId = query.ProblemId,
            Skip = query.Skip,
            Take = query.Take,
            ContestId = query.ContestId,
            TaskLabel = query.ProblemLabel,
            UserId = query.UserId
        };

        var isAdmin = this.User.IsAdmin();

        var result = await this.submitsService.SearchAsync(submitsQuery, !isAdmin);

        return this.Ok(result);
    }

    /// <summary>
    /// Get submit results for problem
    /// </summary>
    /// <param name="problemId">Problem id</param>
    /// <param name="skip">Submit results to skip</param>
    /// <param name="take">Submit result to take</param>
    /// <returns></returns>
    [Authorize]
    [HttpGet("problems/{problemId:long}/submits")]
    [ProducesResponseType(typeof(SubmitResultsList), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProblemSubmits(
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

        var isAdmin = this.User.IsAdmin();

        var result = await this.submitsService.SearchAsync(query, !isAdmin);

        return this.Ok(result);
    }

    /// <summary>
    /// Submit solution
    /// </summary>
    /// <param name="submitSolution"><inheritdoc cref="Judge.Web.Client.Submits.SubmitSolution"/></param>
    [Authorize]
    [HttpPut("submits")]
    [ProducesResponseType(typeof(SubmitSolutionResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SubmitSolution([FromForm] SubmitSolution submitSolution)
    {
        var userId = this.User.GetUserId();
        var userInfo = new SubmitUserInfo(userId, this.Request.Host.ToString());
        try
        {
            var id = await this.submitsService.SaveAsync(submitSolution, userInfo);
            return this.Ok(new SubmitSolutionResult { Id = id });
        }
        catch (InvalidOperationException ex)
        {
            return this.BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Get submit result by id
    /// </summary>
    /// <param name="id">Submit result id</param>
    [Authorize(AuthorizationPolicies.AdminPolicy)]
    [HttpGet("submits/{id:long}")]
    [ProducesResponseType(typeof(SubmitResultExtendedInfo), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSubmit([FromRoute] long id)
    {
        var result = await this.submitsService.GetResultAsync(id);
        if (result == null)
            return this.NotFound();

        return this.Ok(result);
    }

    /// <summary>
    /// Rejudge submit
    /// </summary>
    /// <param name="id">Submit result id</param>
    [Authorize(AuthorizationPolicies.AdminPolicy)]
    [HttpPost("submits/{id:long}/rejudge")]
    [ProducesResponseType(typeof(SubmitResultExtendedInfo), StatusCodes.Status200OK)]
    public async Task<IActionResult> Rejudge([FromRoute] long id)
    {
        var result = await this.submitsService.RejudgeAsync(id);
        if (result == null)
            return this.NotFound();

        return this.Ok(result);
    }
}
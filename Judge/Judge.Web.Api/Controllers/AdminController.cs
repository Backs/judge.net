using System.Threading.Tasks;
using Judge.Services;
using Judge.Web.Api.Authorization;
using Judge.Web.Client.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Judge.Web.Api.Controllers;

/// <summary>
/// Admin
/// </summary>
[Authorize(AuthorizationPolicies.AdminPolicy)]
[Route("api/admin")]
[ApiController]
public class AdminController : ControllerBase
{
    private readonly ILanguageService languageService;
    private readonly IProblemsService problemsService;

    public AdminController(ILanguageService languageService, IProblemsService problemsService)
    {
        this.languageService = languageService;
        this.problemsService = problemsService;
    }


    [HttpGet("languages")]
    [ProducesResponseType(typeof(LanguageList), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetLanguages()
    {
        var result = await this.languageService.GetListAsync();
        return this.Ok(result);
    }

    [HttpGet("problems")]
    [ProducesResponseType(typeof(AllProblemsList), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProblems([FromQuery] int skip = 0, [FromQuery] int take = 100)
    {
        var result = await this.problemsService.GetAllAsync(skip, take);
        return this.Ok(result);
    }
}
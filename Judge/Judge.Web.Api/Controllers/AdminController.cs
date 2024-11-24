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
    private ILanguageService languageService;

    public AdminController(ILanguageService languageService)
    {
        this.languageService = languageService;
    }


    [HttpGet("languages")]
    [ProducesResponseType(typeof(LanguageList), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetLanguages()
    {
        var result = await this.languageService.GetListAsync();
        return this.Ok(result);
    }
}
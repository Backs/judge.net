using System.Web.Mvc;
using Judge.Application.Interfaces;

namespace Judge.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly ISecurityService _securityService;

        public AccountController(ISecurityService securityService)
        {
            _securityService = securityService;
        }

        public ActionResult Login()
        {
            return View();
        }
    }
}
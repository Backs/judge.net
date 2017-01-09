using System.Web.Mvc;
using Judge.Application.Interfaces;
using Judge.Application.ViewModels.Account;
using Microsoft.AspNet.Identity.Owin;

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

        public ActionResult Logout()
        {
            _securityService.SignOut();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = _securityService.SignIn(model.Email, model.Password, model.RememberMe);
            if (result == SignInStatus.Success)
            {
                return RedirectToLocal(returnUrl);
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            _securityService.Register(model);

            return RedirectToAction("Index", "Home");
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
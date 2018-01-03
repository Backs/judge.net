using System.Web.Mvc;
using Judge.Application.Interfaces;

namespace Judge.Web.Controllers
{
    public sealed class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            this._userService = userService;
        }

        public ActionResult Statistics(long id)
        {
            var user = _userService.GetUserInfo(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            return View(user);
        }
    }
}
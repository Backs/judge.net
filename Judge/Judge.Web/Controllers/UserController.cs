namespace Judge.Web.Controllers
{
    using System.Web.Mvc;
    using Judge.Application.Interfaces;

    public sealed class UserController : Controller
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        public ActionResult Statistics(long id)
        {
            var user = this.userService.GetUserInfo(id);
            if (user == null)
            {
                return this.HttpNotFound();
            }

            return this.View(user);
        }
    }
}
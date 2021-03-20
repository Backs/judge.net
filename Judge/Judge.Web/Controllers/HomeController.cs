namespace Judge.Web.Controllers
{
    using System.Web.Mvc;

    public sealed class HomeController : Controller
    {
        public ActionResult Index()
        {
            return this.View();
        }
    }
}
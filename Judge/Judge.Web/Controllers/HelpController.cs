using System.Web.Mvc;

namespace Judge.Web.Controllers
{
    public class HelpController : Controller
    {
        public ActionResult Index()
        {
            var text = System.IO.File.ReadAllText(Server.MapPath("~/Content/Help/ru.md"));
            return View((object)text);
        }
    }
}
namespace Judge.Web.Controllers
{
    using System.Web.Mvc;

    public sealed class HelpController : Controller
    {
        public ActionResult Index()
        {
            var text = System.IO.File.ReadAllText(this.Server.MapPath("~/Content/Help/ru.md"));
            return this.View((object)text);
        }

        public ActionResult About()
        {
            var text = System.IO.File.ReadAllText(this.Server.MapPath("~/Content/About/ru.md"));
            return this.View((object)text);
        }
    }
}
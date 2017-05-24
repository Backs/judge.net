using System.Web.Mvc;
using Judge.Application.Interfaces;

namespace Judge.Web.Controllers
{
    public sealed class ContestsController : Controller
    {
        private readonly IContestsService _contestsService;

        public ContestsController(IContestsService contestsService)
        {
            _contestsService = contestsService;
        }

        public ActionResult Index()
        {
            var model = _contestsService.GetContests();
            return View(model);
        }

        public ActionResult Tasks(long id)
        {
            return View();
        }

        public ActionResult Results(long id)
        {
            return View();
        }
    }
}
using System.Web.Mvc;
using Judge.Application.Interfaces;

namespace Judge.Web.Controllers
{
    public class ProblemsController : Controller
    {
        private readonly IProblemsService _problemsService;

        public ProblemsController(IProblemsService problemsService)
        {
            _problemsService = problemsService;
        }


        public ActionResult Index()
        {
            var model = _problemsService.GetProblemsList(1, 40);
            return View(model);
        }

        public ActionResult Statement(long id)
        {
            var model = _problemsService.GetStatement(id);
            return View(model);
        }
    }
}
using System.Web.Mvc;
using Judge.Application.Interfaces;
using Judge.Application.ViewModels.Submit;

namespace Judge.Web.Controllers
{
    public class ProblemsController : Controller
    {
        private readonly IProblemsService _problemsService;
        private readonly ISubmitSolutionService _submitSolutionService;

        public ProblemsController(IProblemsService problemsService, ISubmitSolutionService submitSolutionService)
        {
            _problemsService = problemsService;
            _submitSolutionService = submitSolutionService;
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

        [HttpGet]
        public PartialViewResult SubmitSolution(long problemId)
        {
            var languages = _submitSolutionService.GetLanguages();
            var model = new SubmitSolutionViewModel { Languages = languages, ProblemId = problemId };
            return PartialView("Submit/_SubmitSolution", model);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SubmitSolution(SubmitSolutionViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.Success = true;
                _submitSolutionService.SubmitSolution(model.ProblemId, model.SelectedLanguage, model.File);
            }
            else
            {
                model.Success = false;
            }
            model.Languages = _submitSolutionService.GetLanguages();

            return PartialView("Submit/_SubmitSolution", model);
        }
    }
}
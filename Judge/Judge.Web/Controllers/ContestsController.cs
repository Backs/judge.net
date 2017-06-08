using System.Web.Mvc;
using Judge.Application.Interfaces;
using Judge.Application.ViewModels.Contests;
using Judge.Web.Services;
using Microsoft.AspNet.Identity;

namespace Judge.Web.Controllers
{
    public sealed class ContestsController : Controller
    {
        private readonly IContestsService _contestsService;
        private readonly ISubmitSolutionService _submitSolutionService;
        private readonly ISessionService _sessionService;

        public ContestsController(IContestsService contestsService, ISubmitSolutionService submitSolutionService, ISessionService sessionService)
        {
            _contestsService = contestsService;
            _submitSolutionService = submitSolutionService;
            _sessionService = sessionService;
        }

        public ActionResult Index()
        {
            var model = _contestsService.GetContests();
            return View(model);
        }

        public ActionResult Tasks(int id)
        {
            long? userId = null;
            if (User.Identity.IsAuthenticated)
            {
                userId = User.Identity.GetUserId<long>();
            }
            var model = _contestsService.GetTasks(id, userId);
            return View(model);
        }

        public ActionResult Task(int contestId, string label)
        {
            var model = _contestsService.GetStatement(contestId, label);
            if (model == null)
                return HttpNotFound();

            if (model.Contest.IsNotStarted)
            {
                return View("NotStartedContest");
            }

            return View(model);
        }

        public ActionResult Results(int id)
        {
            var model = _contestsService.GetResults(id);
            return View(model);
        }

        [Authorize]
        public ActionResult SubmitSolution(int contestId, string label)
        {
            var languages = _submitSolutionService.GetLanguages();
            var model = new SubmitContestSolutionViewModel
            {
                Languages = languages,
                Label = label,
                ContestId = contestId,
                SelectedLanguage = _sessionService.GetSelectedLanguage()
            };
            return PartialView("Contests/_SubmitSolution", model);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [Authorize]
        public ActionResult SubmitSolution(SubmitContestSolutionViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.Success = true;
                var userId = User.Identity.GetUserId<long>();
                _contestsService.SubmitSolution(model.ContestId, model.Label, model.SelectedLanguage, model.File, userId);

                _sessionService.SaveSelectedLanguage(model.SelectedLanguage);

                return Redirect(Request.UrlReferrer.ToString());
            }

            model.Success = false;
            model.Languages = _submitSolutionService.GetLanguages();
            return PartialView("Contests/_SubmitSolution", model);
        }

        [Authorize]
        public ActionResult UserSubmitQueue(int contestId, string label, int? page)
        {
            var userId = User.Identity.GetUserId<long>();
            var model = _contestsService.GetSubmitQueue(userId, contestId, label, page ?? 1, 10);
            return PartialView("Submits/_SubmitQueue", model);
        }
    }
}
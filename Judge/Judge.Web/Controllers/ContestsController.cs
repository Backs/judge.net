namespace Judge.Web.Controllers
{
    using System.Web.Mvc;
    using Judge.Application.Interfaces;
    using Judge.Application.ViewModels;
    using Judge.Application.ViewModels.Contests;
    using Judge.Web.Services;
    using Microsoft.AspNet.Identity;

    public sealed class ContestsController : Controller
    {
        private readonly IContestsService contestsService;
        private readonly ISubmitSolutionService submitSolutionService;
        private readonly ISessionService sessionService;

        public ContestsController(
            IContestsService contestsService,
            ISubmitSolutionService submitSolutionService,
            ISessionService sessionService)
        {
            this.contestsService = contestsService;
            this.submitSolutionService = submitSolutionService;
            this.sessionService = sessionService;
        }

        public ActionResult Index()
        {
            var model = this.contestsService.GetContests(false);
            return this.View(model);
        }

        public ActionResult Tasks(int id)
        {
            long? userId = null;
            if (this.User.Identity.IsAuthenticated)
            {
                userId = this.User.Identity.GetUserId<long>();
            }
            var model = this.contestsService.GetTasks(id, userId);
            return this.View(model);
        }

        public ActionResult Task(int contestId, string label)
        {
            var model = this.contestsService.GetStatement(contestId, label);
            if (model == null)
                return this.HttpNotFound();

            if (model.Contest.IsNotStarted)
            {
                return this.View("NotStartedContest");
            }

            return this.View(model);
        }

        public ActionResult Results(int id)
        {
            var model = this.contestsService.GetResults(id);
            return this.View(model);
        }

        [Authorize]
        public ActionResult SubmitSolution(int contestId, string label)
        {
            var languages = this.submitSolutionService.GetLanguages(contestId, label, this.User.Identity.GetUserId<long>());
            var model = new SubmitContestSolutionViewModel
            {
                Languages = languages,
                Label = label,
                ContestId = contestId,
                SelectedLanguage = this.sessionService.GetSelectedLanguage()
            };
            return this.PartialView("Contests/_SubmitSolution", model);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [Authorize]
        public ActionResult SubmitSolution(SubmitContestSolutionViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                model.Success = true;
                var userId = this.User.Identity.GetUserId<long>();
                var userHost = this.Request.UserHostAddress;
                var sessionId = this.Session.SessionID;
                var userInfo = new UserInfo(userId, userHost, sessionId);
                this.contestsService.SubmitSolution(model.ContestId, model.Label, model.SelectedLanguage, model.File, userInfo);

                this.sessionService.SaveSelectedLanguage(model.SelectedLanguage);

                return this.Redirect(this.Request.UrlReferrer.ToString());
            }

            model.Success = false;
            model.Languages = this.submitSolutionService.GetLanguages(model.ContestId, model.Label, this.User.Identity.GetUserId<long>());
            return this.PartialView("Contests/_SubmitSolution", model);
        }

        [Authorize]
        public ActionResult UserSubmitQueue(int contestId, string label, int? page)
        {
            var userId = this.User.Identity.GetUserId<long>();
            var model = this.contestsService.GetSubmitQueue(userId, contestId, label, page ?? 1, 10);
            return this.PartialView("Submits/_SubmitQueue", model);
        }
    }
}
namespace Judge.Web.Controllers
{
    using System.Security.Authentication;
    using System.Web;
    using System.Web.Mvc;
    using Judge.Application.Interfaces;
    using Judge.Application.ViewModels;
    using Judge.Application.ViewModels.Submit;
    using Judge.Web.Services;
    using Microsoft.AspNet.Identity;

    public sealed class ProblemsController : Controller
    {
        private readonly IProblemsService problemsService;
        private readonly ISubmitSolutionService submitSolutionService;
        private readonly ISessionService sessionService;
        private readonly int pageSize = 20;

        public ProblemsController(IProblemsService problemsService, ISubmitSolutionService submitSolutionService, ISessionService sessionService)
        {
            this.problemsService = problemsService;
            this.submitSolutionService = submitSolutionService;
            this.sessionService = sessionService;
        }

        public ActionResult Index(int? page)
        {
            long? userId = null;
            if (this.User.Identity.IsAuthenticated)
            {
                userId = this.User.Identity.GetUserId<long>();
            }
            var model = this.problemsService.GetProblemsList(page ?? 1, this.pageSize, userId, true);
            return this.View(model);
        }

        public ActionResult Statement(long id)
        {
            var isAdmin = this.User.IsInRole("admin");

            var model = this.problemsService.GetStatement(id, isAdmin);
            if (model == null)
            {
                return this.HttpNotFound();
            }

            return this.View(model);
        }

        [HttpGet]
        public PartialViewResult SubmitSolution(long problemId)
        {
            var languages = this.submitSolutionService.GetLanguages();
            var model = new SubmitSolutionViewModel
            {
                Languages = languages,
                ProblemId = problemId,
                SelectedLanguage = this.sessionService.GetSelectedLanguage()
            };
            return this.PartialView("Submits/_SubmitSolution", model);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [Authorize]
        public ActionResult SubmitSolution(SubmitSolutionViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                model.Success = true;
                var userId = this.User.Identity.GetUserId<long>();
                var userHost = this.Request.UserHostAddress;
                var sessionId = this.Session.SessionID;
                var userInfo = new UserInfo(userId, userHost, sessionId);
                this.submitSolutionService.SubmitSolution(model.ProblemId, model.SelectedLanguage, model.File, userInfo);

                this.sessionService.SaveSelectedLanguage(model.SelectedLanguage);

                return this.Redirect(this.Request.UrlReferrer.ToString());
            }

            model.Success = false;
            model.Languages = this.submitSolutionService.GetLanguages();
            return this.PartialView("Submits/_SubmitSolution", model);
        }

        [Authorize]
        public ActionResult Solution(long submitResultId)
        {
            var userId = this.User.Identity.GetUserId<long>();

            try
            {
                var model = this.submitSolutionService.GetSolution(submitResultId, userId);
                if (model == null)
                {
                    return this.HttpNotFound();
                }

                return this.View(model);
            }
            catch (AuthenticationException)
            {
                throw new HttpException(401, "AuthenticationException");
            }
        }
    }
}
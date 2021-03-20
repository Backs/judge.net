namespace Judge.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Judge.Application.Interfaces;
    using Judge.Application.ViewModels.Admin.Contests;
    using Judge.Application.ViewModels.Admin.Languages;
    using Judge.Application.ViewModels.Admin.Problems;
    using Judge.Application.ViewModels.Admin.Submits;
    using Judge.Application.ViewModels.Admin.Users;
    using Judge.Model.SubmitSolution;

    [Authorize(Roles = "admin")]
    public sealed class AdminController : Controller
    {
        private readonly IAdminService adminService;
        private readonly IProblemsService problemsService;
        private readonly IContestsService contestsService;
        private readonly ISecurityService securityService;

        public AdminController(IAdminService adminService, IProblemsService problemsService,
        IContestsService contestsService, ISecurityService securityService)
        {
            this.adminService = adminService;
            this.problemsService = problemsService;
            this.contestsService = contestsService;
            this.securityService = securityService;
        }

        public ActionResult Index()
        {
            return this.View();
        }

        [HttpGet]
        public ActionResult Languages()
        {
            var model = this.adminService.GetLanguages();

            return this.View(model);
        }

        [HttpPost]
        public ActionResult Languages(List<LanguageEditViewModel> languages)
        {
            languages.ForEach(o => this.TryValidateModel(o));
            if (!this.ModelState.IsValid)
            {
                return this.View(languages);
            }

            this.adminService.SaveLanguages(languages);

            return this.RedirectToAction("Index");
        }

        public PartialViewResult Language()
        {
            return this.PartialView("Admin/Languages/_LanguageEditView", new LanguageEditViewModel());
        }

        [HttpGet]
        public ActionResult Submits(SubmitsFilter filter)
        {
            filter = filter ?? new SubmitsFilter();

            var submits = this.adminService.GetSubmitQueue(filter.SelectedLanguage, filter.SelectedStatus);
            var languages = this.adminService.GetLanguages();

            var statuses = Enum.GetValues(typeof(SubmitStatus)).Cast<SubmitStatus>().Select(o => new SubmitStatusItem { Id = o, Name = o.GetDescription() });

            var model = new SubmitsQueue
            {
                Submits = submits,
                Languages = new[] { new LanguageItem { Name = "Все" } }.Concat(languages.Select(o => new LanguageItem { Id = o.Id, Name = o.Name })),
                SelectedLanguage = filter.SelectedLanguage,
                Statuses = new[] { new SubmitStatusItem { Name = "Все" } }.Concat(statuses),
                SelectedStatus = filter.SelectedStatus
            };

            return this.View(model);
        }

        [HttpGet]
        public ActionResult EditProblem(long? id)
        {
            if (id == null)
            {
                return this.View(new EditProblemViewModel());
            }

            var model = this.adminService.GetProblem(id.Value);
            return this.View(model);
        }

        [HttpPost]
        public ActionResult EditProblem(EditProblemViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var id = this.adminService.SaveProblem(model);
                return this.RedirectToAction("EditProblem", new { id });
            }

            return this.View(model);
        }

        public ActionResult Problems(int? page)
        {
            var model = this.problemsService.GetProblemsList(page ?? 1, 20, null, false);
            return this.View(model);
        }

        public ActionResult Contests()
        {
            var contests = this.contestsService.GetContests(true);
            return this.View(contests);
        }

        public ActionResult EditContest(int? id)
        {
            var problems = this.problemsService.GetAllProblems();
            this.ViewBag.Problems = problems;

            var model = id != null ? this.adminService.GetContest(id.Value) : new EditContestViewModel();
            return this.View(model);
        }

        [HttpPost]
        public ActionResult EditContest(EditContestViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                model.Id = this.adminService.SaveContest(model);
                return this.RedirectToAction("EditContest", new { id = model.Id });
            }

            var problems = this.problemsService.GetAllProblems();
            this.ViewBag.Problems = problems;
            return this.View(model);
        }

        [HttpGet]
        public ActionResult Users()
        {
            var model = this.adminService.GetUsers();
            return this.View(model);
        }

        [HttpGet]
        public ActionResult EditUser(long id)
        {
            var model = this.adminService.GetUser(id);

            if (model == null)
            {
                return this.HttpNotFound();
            }

            return this.View(model);
        }

        public ActionResult EditUser(UserEditViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                this.securityService.UpdateUser(model);
                return this.RedirectToAction("Users");
            }

            return this.View(model);
        }

        public PartialViewResult Task()
        {
            var problems = this.problemsService.GetAllProblems();
            this.ViewBag.Problems = problems;
            return this.PartialView("Admin/Contests/_TaskEditView", new TaskEditViewModel());
        }
    }
}
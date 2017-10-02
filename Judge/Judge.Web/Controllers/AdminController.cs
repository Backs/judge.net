using System.Collections.Generic;
using System.Web.Mvc;
using Judge.Application.Interfaces;
using Judge.Application.ViewModels.Admin.Languages;
using Judge.Application.ViewModels.Admin.Problems;

namespace Judge.Web.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;
        private readonly IProblemsService _problemsService;

        public AdminController(IAdminService adminService, IProblemsService problemsService)
        {
            _adminService = adminService;
            _problemsService = problemsService;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Languages()
        {
            var model = _adminService.GetLanguages();

            return View(model);
        }

        [HttpPost]
        public ActionResult Languages(List<LanguageEditViewModel> languages)
        {
            languages.ForEach(o => TryValidateModel(o));
            if (!ModelState.IsValid)
            {
                return View(languages);
            }
            _adminService.SaveLanguages(languages);

            return RedirectToAction("Index");
        }

        public PartialViewResult Language()
        {
            return PartialView("Admin/Languages/_LanguageEditView", new LanguageEditViewModel());
        }

        public ActionResult Submits()
        {
            var model = _adminService.GetSubmitQueue();
            return View(model);
        }

        [HttpGet]
        public ActionResult EditProblem(long? id)
        {
            if (id == null)
            {
                return View(new EditProblemViewModel());
            }
            var model = _adminService.GetProblem(id.Value);
            return View(model);
        }

        [HttpPost]
        public ActionResult EditProblem(EditProblemViewModel model)
        {
            if (ModelState.IsValid)
            {
                var id = _adminService.SaveProblem(model);
                return RedirectToAction("EditProblem", new { id });
            }
            return View(model);
        }

        public ActionResult Problems(int? page)
        {
            var model = _problemsService.GetProblemsList(page ?? 1, 20, null);
            return View(model);
        }
    }
}
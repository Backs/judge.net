using System.Collections.Generic;
using System.Web.Mvc;
using Judge.Application.Interfaces;
using Judge.Application.ViewModels.Admin.Languages;

namespace Judge.Web.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
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
    }
}
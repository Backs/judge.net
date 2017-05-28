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

        public ActionResult Tasks(int id)
        {
            var model = _contestsService.GetTasks(id);
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

        public ActionResult Results(long id)
        {
            return View();
        }

        [Authorize]
        public ActionResult SubmitSolution(int contestId, long problemId)
        {
            return new EmptyResult();
        }

        [Authorize]
        public ActionResult UserSubmitQueue(int contestId, long problemId, int page)
        {
            return new EmptyResult();
        }
    }
}
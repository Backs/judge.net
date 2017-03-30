using System.Web.Mvc;
using Judge.Application.Interfaces;
using Microsoft.AspNet.Identity;

namespace Judge.Web.Controllers
{
    public class SubmitsController : Controller
    {
        private readonly ISubmitQueueService _submitQueueService;

        public SubmitsController(ISubmitQueueService submitQueueService)
        {
            _submitQueueService = submitQueueService;
        }

        public ViewResult Index()
        {
            return View();
        }

        public PartialViewResult UserSubmitQueue(long problemId)
        {
            var userId = User.Identity.GetUserId<long>();
            var model = _submitQueueService.GetSubmitQueue(userId, problemId);
            return PartialView("Submits/_SubmitQueue", model);
        }

        public PartialViewResult FullSubmitQueue()
        {
            long? userId = null;
            if (User.Identity.IsAuthenticated)
            {
                userId = User.Identity.GetUserId<long>();
            }
            var model = _submitQueueService.GetSubmitQueue(userId);
            return PartialView("Submits/_SubmitQueue", model);
        }
    }
}
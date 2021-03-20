namespace Judge.Web.Controllers
{
    using System.Web.Mvc;
    using Judge.Application.Interfaces;
    using Microsoft.AspNet.Identity;

    public sealed class SubmitsController : Controller
    {
        private readonly ISubmitQueueService submitQueueService;

        public SubmitsController(ISubmitQueueService submitQueueService)
        {
            this.submitQueueService = submitQueueService;
        }

        public ViewResult Index()
        {
            return this.View();
        }

        public PartialViewResult UserSubmitQueue(long problemId, int? page)
        {
            var userId = this.User.Identity.GetUserId<long>();
            var model = this.submitQueueService.GetSubmitQueue(userId, problemId, page ?? 1, 10);
            return this.PartialView("Submits/_SubmitQueue", model);
        }

        public PartialViewResult FullSubmitQueue(int? page)
        {
            long? userId = null;
            if (this.User.Identity.IsAuthenticated)
            {
                userId = this.User.Identity.GetUserId<long>();
            }
            var model = this.submitQueueService.GetSubmitQueue(userId, page ?? 1, 20);
            return this.PartialView("Submits/_SubmitQueue", model);
        }
    }
}
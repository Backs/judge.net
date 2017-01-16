using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Judge.Application.Interfaces;
using Microsoft.AspNet.Identity;

namespace Judge.Web.Controllers
{
    public class SubmitController : Controller
    {
        private readonly ISubmitQueueService _submitQueueService;

        public SubmitController(ISubmitQueueService submitQueueService)
        {
            _submitQueueService = submitQueueService;
        }

        public PartialViewResult UserSubmitQueue(long problemId)
        {
            var userId = User.Identity.GetUserId<long>();
            var model = _submitQueueService.GetSubmitQueue(userId, problemId);
            return PartialView("Submit/_SubmitQueue", model);
        }
    }
}
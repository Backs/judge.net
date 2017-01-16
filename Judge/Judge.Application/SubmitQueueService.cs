using System.Linq;
using Judge.Application.Interfaces;
using Judge.Application.ViewModels.Submit;
using Judge.Data;
using Judge.Model.SubmitSolution;

namespace Judge.Application
{
    internal sealed class SubmitQueueService : ISubmitQueueService
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        public SubmitQueueService(IUnitOfWorkFactory unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        public SubmitQueueViewModel GetSubmitQueue(long? userId, long problemId)
        {
            using (var uow = _unitOfWorkFactory.GetUnitOfWork(false))
            {
                var repository = uow.GetRepository<ISubmitResultRepository>();
                var submits = repository.GetLastSubmits(userId, problemId, 10);

                var items = submits.Select(o => new SubmitQueueItem
                {
                    Language = "C++", //TODO
                    PassedTests = o.PassedTests,
                    ProblemId = o.Submit.ProblemId,
                    ProblemName = "problem name", //TODO
                    Status = o.Status,
                    SubmitResultId = o.Id,
                    SubmitTime = o.Submit.SubmitDateUtc,
                    TotalBytes = o.TotalBytes,
                    TotalMilliseconds = o.TotalMilliseconds,
                    UserId = o.Submit.UserId,
                    UserName = "user name" //TODO
                });

                var model = new SubmitQueueViewModel(items);

                return model;
            }
        }
    }
}

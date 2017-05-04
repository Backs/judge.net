using System.Linq;
using Judge.Application.Interfaces;
using Judge.Application.ViewModels.Submit;
using Judge.Data;
using Judge.Model.Account;
using Judge.Model.CheckSolution;
using Judge.Model.Configuration;
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

        public SubmitQueueViewModel GetSubmitQueue(long userId, long problemId, int page, int pageSize)
        {
            using (var uow = _unitOfWorkFactory.GetUnitOfWork(false))
            {
                var submitResultRepository = uow.GetRepository<ISubmitResultRepository>();
                var languageRepository = uow.GetRepository<ILanguageRepository>();
                var taskRepository = uow.GetRepository<ITaskNameRepository>();
                var userRepository = uow.GetRepository<IUserRepository>();

                var languages = languageRepository.GetLanguages().ToDictionary(o => o.Id, o => o.Name);
                var submits = submitResultRepository.GetSubmits(userId, problemId, page, pageSize);
                var count = submitResultRepository.Count(problemId, userId);

                var task = taskRepository.GetTasks(new[] { problemId }).First();
                var user = userRepository.GetUsers(new[] { userId }).First();

                var items = submits.Select(o => new SubmitQueueItem(o, languages[o.Submit.LanguageId], task.Name, user.UserName) { ResultsEnabled = true });

                var model = new SubmitQueueViewModel(items)
                {
                    Pagination = new ViewModels.PaginationViewModel
                    {
                        CurrentPage = page,
                        PageSize = pageSize,
                        TotalPages = (count + pageSize - 1) / pageSize
                    }
                };

                return model;
            }
        }

        public SubmitQueueViewModel GetSubmitQueue(long? userId, int page, int pageSize)
        {
            using (var uow = _unitOfWorkFactory.GetUnitOfWork(false))
            {
                var submitResultRepository = uow.GetRepository<ISubmitResultRepository>();
                var languageRepository = uow.GetRepository<ILanguageRepository>();
                var taskRepository = uow.GetRepository<ITaskNameRepository>();
                var userRepository = uow.GetRepository<IUserRepository>();

                var languages = languageRepository.GetLanguages().ToDictionary(o => o.Id, o => o.Name);
                var submits = submitResultRepository.GetSubmits(null, null, page, pageSize).ToArray();

                var tasks = taskRepository.GetTasks(submits.Select(o => o.Submit.ProblemId)).ToDictionary(o => o.Id, o => o.Name);
                var users = userRepository.GetUsers(submits.Select(o => o.Submit.UserId)).ToDictionary(o => o.Id, o => o.UserName);

                var items = submits.Select(o => new SubmitQueueItem(o, languages[o.Submit.LanguageId], tasks[o.Submit.ProblemId], users[o.Submit.UserId])
                {
                    ResultsEnabled = userId == o.Submit.UserId
                });

                var count = submitResultRepository.Count(null, null);

                var model = new SubmitQueueViewModel(items)
                {
                    Pagination = new ViewModels.PaginationViewModel
                    {
                        CurrentPage = page,
                        PageSize = pageSize,
                        TotalPages = (count + pageSize - 1) / pageSize
                    }
                };

                return model;
            }
        }
    }
}

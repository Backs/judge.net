using System.Linq;
using System.Security.Principal;
using Judge.Application.Interfaces;
using Judge.Application.ViewModels.Submit;
using Judge.Data;
using Judge.Model.SubmitSolution;

namespace Judge.Application
{
    internal sealed class SubmitQueueService : ISubmitQueueService
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly IPrincipal _principal;

        public SubmitQueueService(IUnitOfWorkFactory unitOfWorkFactory, IPrincipal principal)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _principal = principal;
        }

        public SubmitQueueViewModel GetSubmitQueue(long userId, long problemId, int page, int pageSize)
        {
            using (var uow = _unitOfWorkFactory.GetUnitOfWork())
            {
                var submitResultRepository = uow.SubmitResultRepository;
                var languageRepository = uow.LanguageRepository;
                var taskRepository = uow.TaskNameRepository;
                var userRepository = uow.UserRepository;

                var languages = languageRepository.GetLanguages().ToDictionary(o => o.Id, o => o.Name);

                var specification = new UserProblemSpecification(userId, problemId);

                var submits = submitResultRepository.GetSubmits(specification, page, pageSize);
                var count = submitResultRepository.Count(specification);

                var task = taskRepository.GetTasks(new[] { problemId }).First();
                var user = userRepository.GetUsers(new[] { userId }).First();

                var items = submits.Select(o => new SubmitQueueItem(o, languages[o.Submit.LanguageId], task.Name, user.UserName) { ResultsEnabled = true })
                    .ToArray();

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
            using (var uow = _unitOfWorkFactory.GetUnitOfWork())
            {
                var submitResultRepository = uow.SubmitResultRepository;
                var languageRepository = uow.LanguageRepository;
                var taskRepository = uow.TaskNameRepository;
                var userRepository = uow.UserRepository;

                var languages = languageRepository.GetLanguages().ToDictionary(o => o.Id, o => o.Name);
                var submits = submitResultRepository.GetSubmits(AllProblemsSpecification.Instance, page, pageSize).ToArray();

                var tasks = taskRepository.GetTasks(submits.Select(o => o.Submit.ProblemId).Distinct()).ToDictionary(o => o.Id, o => o.Name);
                var users = userRepository.GetUsers(submits.Select(o => o.Submit.UserId).Distinct()).ToDictionary(o => o.Id, o => o.UserName);

                var hasPermission = _principal.IsInRole("admin");

                var items = submits.Select(o => new SubmitQueueItem(o, languages[o.Submit.LanguageId], tasks[o.Submit.ProblemId], users[o.Submit.UserId])
                {
                    ResultsEnabled = userId == o.Submit.UserId || hasPermission
                }).ToArray();

                var count = submitResultRepository.Count(AllProblemsSpecification.Instance);

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

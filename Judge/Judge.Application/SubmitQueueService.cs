namespace Judge.Application
{
    using System.Linq;
    using System.Security.Principal;
    using Judge.Application.Interfaces;
    using Judge.Application.ViewModels.Submit;
    using Judge.Data;
    using Judge.Model.Account;
    using Judge.Model.SubmitSolution;

    internal sealed class SubmitQueueService : ISubmitQueueService
    {
        private readonly IUnitOfWorkFactory unitOfWorkFactory;
        private readonly IPrincipal principal;

        public SubmitQueueService(IUnitOfWorkFactory unitOfWorkFactory, IPrincipal principal)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
            this.principal = principal;
        }

        public SubmitQueueViewModel GetSubmitQueue(long userId, long problemId, int page, int pageSize)
        {
            using (var uow = this.unitOfWorkFactory.GetUnitOfWork())
            {
                var submitResultRepository = uow.SubmitResultRepository;
                var languageRepository = uow.LanguageRepository;
                var taskRepository = uow.TaskRepository;
                var userRepository = uow.UserRepository;

                var languages = languageRepository.GetLanguages(false).ToDictionary(o => o.Id, o => o.Name);

                var specification = new UserProblemSpecification(userId, problemId);

                var submits = submitResultRepository.GetSubmits(specification, page, pageSize);
                var count = submitResultRepository.Count(specification);

                var task = taskRepository.Get(problemId);
                var user = userRepository.Get(userId);

                var items = submits.Select(o => new SubmitQueueItem(o, languages[o.Submit.LanguageId], task, user.UserName) { ResultsEnabled = true })
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
            using (var uow = this.unitOfWorkFactory.GetUnitOfWork())
            {
                var submitResultRepository = uow.SubmitResultRepository;
                var languageRepository = uow.LanguageRepository;
                var taskRepository = uow.TaskRepository;
                var userRepository = uow.UserRepository;

                var languages = languageRepository.GetLanguages(false).ToDictionary(o => o.Id, o => o.Name);
                var submits = submitResultRepository.GetSubmits(ProblemsSpecification.Instance, page, pageSize).ToArray();

                var userSpecification = new UserListSpecification(submits.Select(o => o.Submit.UserId).Distinct());
                var tasks = taskRepository.GetTasks(submits.Select(o => o.Submit.ProblemId).Distinct()).ToDictionary(o => o.Id);
                var users = userRepository.Find(userSpecification).ToDictionary(o => o.Id, o => o.UserName);

                var hasPermission = this.principal.IsInRole("admin");

                var items = submits.Select(o => new SubmitQueueItem(o, languages[o.Submit.LanguageId], tasks[o.Submit.ProblemId], users[o.Submit.UserId])
                {
                    ResultsEnabled = userId == o.Submit.UserId || hasPermission
                }).ToArray();

                var count = submitResultRepository.Count(ProblemsSpecification.Instance);

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

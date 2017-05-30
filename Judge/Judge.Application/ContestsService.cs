using System;
using System.IO;
using System.Linq;
using System.Web;
using Judge.Application.Interfaces;
using Judge.Application.ViewModels.Contests;
using Judge.Application.ViewModels.Contests.ContestsList;
using Judge.Application.ViewModels.Contests.ContestTasks;
using Judge.Application.ViewModels.Submit;
using Judge.Data;
using Judge.Model.Account;
using Judge.Model.Configuration;
using Judge.Model.Contests;
using Judge.Model.SubmitSolution;

namespace Judge.Application
{
    internal sealed class ContestsService : IContestsService
    {
        private readonly IUnitOfWorkFactory _factory;

        public ContestsService(IUnitOfWorkFactory factory)
        {
            _factory = factory;
        }

        public ContestsListViewModel GetContests()
        {
            using (var unitOfWork = _factory.GetUnitOfWork(false))
            {
                var repository = unitOfWork.GetRepository<IContestsRepository>();

                var items = repository.GetList().Select(o => new ContestItem(o));
                return new ContestsListViewModel(items);
            }
        }

        public ContestTasksViewModel GetTasks(int contestId)
        {
            using (var unitOfWork = _factory.GetUnitOfWork(false))
            {
                var contestRepository = unitOfWork.GetRepository<IContestsRepository>();
                var contest = contestRepository.Get(contestId);
                var taskRepository = unitOfWork.GetRepository<IContestTaskRepository>();
                var items = taskRepository.GetTasks(contestId).Select(o => new ContestTaskItem
                {
                    Label = o.TaskName,
                    Name = o.Task.Name,
                    ProblemId = o.Task.Id,
                    Solved = false //TODO: change
                })
                .OrderBy(o => o.Label);

                return new ContestTasksViewModel(items)
                {
                    ContestId = contest.Id,
                    ContestName = contest.Name
                };
            }
        }

        public ContestStatementViewModel GetStatement(int contestId, string label)
        {
            using (var unitOfWork = _factory.GetUnitOfWork(false))
            {
                var contestTaskRepository = unitOfWork.GetRepository<IContestTaskRepository>();

                var task = contestTaskRepository.Get(contestId, label);
                if (task == null)
                    return null;

                var contestRepository = unitOfWork.GetRepository<IContestsRepository>();
                var contest = contestRepository.Get(contestId);

                return new ContestStatementViewModel
                {
                    Id = task.Task.Id,
                    CreationDate = task.Task.CreationDateUtc,
                    MemoryLimitBytes = task.Task.MemoryLimitBytes,
                    Name = task.Task.Name,
                    Statement = task.Task.Statement,
                    TimeLimitMilliseconds = task.Task.TimeLimitMilliseconds,
                    Label = task.TaskName,
                    Contest = new ContestItem(contest)
                };
            }
        }

        public void SubmitSolution(int contestId, string label, int selectedLanguage, HttpPostedFileBase file, long userId)
        {
            if (file == null)
                throw new ArgumentNullException(nameof(file));

            string sourceCode;
            using (var sr = new StreamReader(file.InputStream))
            {
                sourceCode = sr.ReadToEnd();
            }

            using (var unitOfWork = _factory.GetUnitOfWork(true))
            {
                var contest = unitOfWork.GetRepository<IContestsRepository>().Get(contestId);
                if (DateTime.UtcNow < contest.StartTime)
                    throw new InvalidOperationException("Contest not started");

                if (DateTime.UtcNow >= contest.FinishTime)
                    throw new InvalidOperationException("Contest finished");

                var task = unitOfWork.GetRepository<IContestTaskRepository>().Get(contestId, label);

                if (task == null)
                    throw new InvalidOperationException("Task not found");

                var submit = ContestTaskSubmit.Create();

                submit.ProblemId = task.Task.Id;
                submit.ContestId = contestId;
                submit.LanguageId = selectedLanguage;
                submit.UserId = userId;
                submit.FileName = file.FileName;
                submit.SourceCode = sourceCode;

                var submitRepository = unitOfWork.GetRepository<ISubmitRepository>();
                submitRepository.Add(submit);
                unitOfWork.Commit();
            }
        }

        public SubmitQueueViewModel GetSubmitQueue(long userId, int contestId, string label, int page, int pageSize)
        {
            using (var uow = _factory.GetUnitOfWork(false))
            {
                var submitResultRepository = uow.GetRepository<ISubmitResultRepository>();
                var languageRepository = uow.GetRepository<ILanguageRepository>();
                var userRepository = uow.GetRepository<IUserRepository>();
                var contestTaskRepository = uow.GetRepository<IContestTaskRepository>();

                var task = contestTaskRepository.Get(contestId, label);
                
                var languages = languageRepository.GetLanguages().ToDictionary(o => o.Id, o => o.Name);

                var specification = new ContestTaskSpecification(contestId, task.Task.Id, userId);

                var submits = submitResultRepository.GetSubmits(specification, page, pageSize);
                var count = submitResultRepository.Count(specification);

                var user = userRepository.GetUsers(new[] { userId }).First();

                var items = submits.Select(o => new SubmitQueueItem(o, languages[o.Submit.LanguageId], task.Task.Name, user.UserName) { ResultsEnabled = true });

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

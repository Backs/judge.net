using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Judge.Application.Interfaces;
using Judge.Application.ViewModels.Contests;
using Judge.Application.ViewModels.Contests.ContestResult;
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

                var items = repository.GetList().Where(o => o.IsOpened).Select(o => new ContestItem(o));
                return new ContestsListViewModel(items);
            }
        }

        public ContestTasksViewModel GetTasks(int contestId, long? userId)
        {
            using (var unitOfWork = _factory.GetUnitOfWork(false))
            {
                var contestRepository = unitOfWork.GetRepository<IContestsRepository>();
                var contest = contestRepository.Get(contestId);
                var taskRepository = unitOfWork.GetRepository<IContestTaskRepository>();
                var submitResultRepository = unitOfWork.GetRepository<ISubmitResultRepository>();

                var contestTasks = taskRepository.GetTasks(contestId).ToArray();

                var solvedTasks = new HashSet<long>();

                if (userId != null)
                {
                    solvedTasks.UnionWith(submitResultRepository.GetSolvedProblems(new UserContestSolvedProblemsSpecification(contestId, userId.Value, contestTasks.Select(o => o.Task.Id))));
                }

                var items = contestTasks.Select(o => new ContestTaskItem
                {
                    Label = o.TaskName,
                    Name = o.Task.Name,
                    ProblemId = o.Task.Id,
                    Solved = solvedTasks.Contains(o.Task.Id)
                })
                .OrderBy(o => o.Label);

                return new ContestTasksViewModel(items)
                {
                    Contest = new ContestItem(contest)
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

        public void SubmitSolution(int contestId, string label, int selectedLanguage, HttpPostedFileBase file, long userId, string userHost)
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
                submit.UserHost = userHost;

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

                var model = new ContestSubmitQueueViewModel(items)
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

        public ContestResultViewModel GetResults(int id)
        {
            using (var unitOfWork = _factory.GetUnitOfWork(false))
            {
                var contestResultRepository = unitOfWork.GetRepository<IContestResultRepository>();
                var contestTaskRepository = unitOfWork.GetRepository<IContestTaskRepository>();
                var contestRepository = unitOfWork.GetRepository<IContestsRepository>();
                var userRepository = unitOfWork.GetRepository<IUserRepository>();

                var contest = contestRepository.Get(id);
                var tasks = contestTaskRepository.GetTasks(id);
                var results = contestResultRepository.Get(id).ToArray();

                var users = userRepository.GetUsers(results.Select(o => o.UserId).Distinct()).ToDictionary(o => o.Id, o => o.UserName);

                return new ContestResultViewModel
                {
                    Contest = new ContestItem(contest),
                    Tasks = tasks.Select(o => new TaskViewModel { Label = o.TaskName, ProblemId = o.Task.Id }).ToArray(),
                    Users = results.Select(o => new ContestUserResultViewModel
                    {
                        UserId = o.UserId,
                        UserName = users[o.UserId],
                        Tasks = o.TaskResults.Select(t => new ContestTaskResultViewModel(contest.StartTime, t.SubmitDateUtc)
                        {
                            Solved = t.Solved,
                            ProblemId = t.ProblemId,
                            Attempts = t.Attempts
                        }).ToList().AsReadOnly()
                    }).OrderByDescending(o => o.SolvedCount).ThenBy(o => o.Time)
                };
            }
        }
    }
}

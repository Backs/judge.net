using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Judge.Application.Interfaces;
using Judge.Application.ViewModels;
using Judge.Application.ViewModels.Contests;
using Judge.Application.ViewModels.Contests.ContestResult;
using Judge.Application.ViewModels.Contests.ContestsList;
using Judge.Application.ViewModels.Contests.ContestTasks;
using Judge.Application.ViewModels.Submit;
using Judge.Data;
using Judge.Model;
using Judge.Model.Account;
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

        public ContestsListViewModel GetContests(bool showAll)
        {
            using (var unitOfWork = _factory.GetUnitOfWork())
            {
                var repository = unitOfWork.ContestsRepository;

                var specification = showAll ? (ISpecification<Contest>)AllContestsSpecification.Instance : OpenedContestsSpecification.Instance;
                var items = repository.GetList(specification).Select(o => new ContestItem(o)).ToArray();
                return new ContestsListViewModel(items);
            }
        }

        public ContestTasksViewModel GetTasks(int contestId, long? userId)
        {
            using (var unitOfWork = _factory.GetUnitOfWork())
            {
                var contestRepository = unitOfWork.ContestsRepository;
                var contest = contestRepository.Get(contestId);
                var taskRepository = unitOfWork.ContestTaskRepository;
                var submitResultRepository = unitOfWork.SubmitResultRepository;

                var contestTasks = taskRepository.GetTasks(contestId).ToArray();

                var solvedTasks = new HashSet<long>();

                if (userId != null)
                {
                    var solvedProblems = submitResultRepository.GetSolvedProblems(new UserContestSolvedProblemsSpecification(contestId, userId.Value, contestTasks.Select(o => o.Task.Id)));

                    solvedTasks.UnionWith(solvedProblems);
                }

                var items = contestTasks.Select(o => new ContestTaskItem
                {
                    Label = o.TaskName,
                    Name = o.Task.Name,
                    ProblemId = o.Task.Id,
                    Solved = solvedTasks.Contains(o.Task.Id)
                })
                .OrderBy(o => o.Label)
                .ToArray();

                return new ContestTasksViewModel(items)
                {
                    Contest = new ContestItem(contest)
                };
            }
        }

        public ContestStatementViewModel GetStatement(int contestId, string label)
        {
            using (var unitOfWork = _factory.GetUnitOfWork())
            {
                var contestTaskRepository = unitOfWork.ContestTaskRepository;

                var task = contestTaskRepository.Get(contestId, label);
                if (task == null)
                    return null;

                var contestRepository = unitOfWork.ContestsRepository;
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

        public void SubmitSolution(int contestId, string label, int selectedLanguage, HttpPostedFileBase file, UserInfo userInfo)
        {
            if (file == null)
                throw new ArgumentNullException(nameof(file));

            string sourceCode;
            using (var sr = new StreamReader(file.InputStream))
            {
                sourceCode = sr.ReadToEnd();
            }

            using (var unitOfWork = _factory.GetUnitOfWork())
            {
                var contest = unitOfWork.ContestsRepository.Get(contestId);
                if (DateTime.UtcNow < contest.StartTime)
                    throw new InvalidOperationException("Contest not started");

                if (DateTime.UtcNow >= contest.FinishTime)
                    throw new InvalidOperationException("Contest finished");

                var task = unitOfWork.ContestTaskRepository.Get(contestId, label);

                if (task == null)
                    throw new InvalidOperationException("Task not found");

                var submit = ContestTaskSubmit.Create();

                submit.ProblemId = task.Task.Id;
                submit.ContestId = contestId;
                submit.LanguageId = selectedLanguage;
                submit.UserId = userInfo.UserId;
                submit.FileName = Path.GetFileName(file.FileName);
                submit.SourceCode = sourceCode;
                submit.UserHost = userInfo.Host;
                submit.SessionId = userInfo.SessionId;

                var submitRepository = unitOfWork.SubmitRepository;
                submitRepository.Add(submit);
                unitOfWork.Commit();
            }
        }

        public SubmitQueueViewModel GetSubmitQueue(long userId, int contestId, string label, int page, int pageSize)
        {
            using (var uow = _factory.GetUnitOfWork())
            {
                var submitResultRepository = uow.SubmitResultRepository;
                var languageRepository = uow.LanguageRepository;
                var userRepository = uow.UserRepository;
                var contestTaskRepository = uow.ContestTaskRepository;

                var task = contestTaskRepository.Get(contestId, label);

                var languages = languageRepository.GetLanguages().ToDictionary(o => o.Id, o => o.Name);

                var specification = new ContestTaskSpecification(contestId, task.Task.Id, userId);

                var submits = submitResultRepository.GetSubmits(specification, page, pageSize);
                var count = submitResultRepository.Count(specification);

                var user = userRepository.Get(userId);

                var items = submits.Select(o => new SubmitQueueItem(o, languages[o.Submit.LanguageId], task.Task.Name, user.UserName) { ResultsEnabled = true })
                    .ToArray();

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
            using (var unitOfWork = _factory.GetUnitOfWork())
            {
                var contestResultRepository = unitOfWork.ContestResultRepository;
                var contestTaskRepository = unitOfWork.ContestTaskRepository;
                var contestRepository = unitOfWork.ContestsRepository;
                var userRepository = unitOfWork.UserRepository;

                var contest = contestRepository.Get(id);
                var tasks = contestTaskRepository.GetTasks(id);
                var results = contestResultRepository.Get(id).ToArray();

                var userSpecification = new UserListSpecification(results.Select(o => o.UserId).Distinct());
                var users = userRepository.Find(userSpecification).ToDictionary(o => o.Id, o => o.UserName);

                var factory = GetFactory(contest.Rules);

                var userModels = results.Select(o => new ContestUserResultViewModel
                {
                    UserId = o.UserId,
                    UserName = users[o.UserId],
                    Tasks = o.TaskResults.Select(t => factory.Convert(contest, t)).ToDictionary(t => t.ProblemId)
                });

                return new ContestResultViewModel(userModels)
                {
                    Contest = new ContestItem(contest),
                    Tasks = tasks.Select(o => new TaskViewModel { Label = o.TaskName, ProblemId = o.Task.Id }).ToArray()
                };
            }
        }

        private static IContestTaskResultFactory GetFactory(ContestRules rules)
        {
            switch (rules)
            {
                case ContestRules.Acm:
                    return new AcmContestTaskResultFactory();
                case ContestRules.Points:
                    return new PointsContestTaskResultFactory();
                default:
                    throw new ArgumentOutOfRangeException(nameof(rules), rules, null);
            }
        }
    }
}

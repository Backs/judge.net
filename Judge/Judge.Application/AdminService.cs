using Judge.Application.Interfaces;
using Judge.Application.ViewModels.Admin.Contests;
using Judge.Application.ViewModels.Admin.Languages;
using Judge.Application.ViewModels.Admin.Problems;
using Judge.Data;
using Judge.Model.CheckSolution;
using Judge.Model.Contests;
using Judge.Model.Entities;
using Judge.Model.SubmitSolution;
using System.Collections.Generic;
using System.Linq;
using Judge.Application.ViewModels.Admin.Users;
using Judge.Model.Account;
using SubmitQueueItem = Judge.Application.ViewModels.Admin.Submits.SubmitQueueItem;

namespace Judge.Application
{
    internal sealed class AdminService : IAdminService
    {
        private readonly IUnitOfWorkFactory _factory;

        public AdminService(IUnitOfWorkFactory factory)
        {
            _factory = factory;
        }

        public List<LanguageEditViewModel> GetLanguages()
        {
            using (var uow = _factory.GetUnitOfWork())
            {
                var repository = uow.LanguageRepository;
                return repository.GetLanguages().Select(o => new LanguageEditViewModel
                {
                    Id = o.Id,
                    CompilerOptionsTemplate = o.CompilerOptionsTemplate,
                    CompilerPath = o.CompilerPath,
                    Description = o.Description,
                    IsCompilable = o.IsCompilable,
                    IsHidden = o.IsHidden,
                    Name = o.Name,
                    OutputFileTemplate = o.OutputFileTemplate,
                    RunStringFormat = o.RunStringFormat
                })
                .ToList();
            }
        }

        public void SaveLanguages(ICollection<LanguageEditViewModel> languages)
        {
            using (var uow = _factory.GetUnitOfWork())
            {
                var repository = uow.LanguageRepository;
                var databaseLanguages = repository.GetLanguages();

                foreach (var databaseLanguage in databaseLanguages)
                {
                    var language = languages.FirstOrDefault(o => o.Id == databaseLanguage.Id);
                    if (language == null)
                    {
                        repository.Delete(databaseLanguage);
                        continue;
                    }

                    databaseLanguage.CompilerOptionsTemplate = language.CompilerOptionsTemplate;
                    databaseLanguage.CompilerPath = language.CompilerPath;
                    databaseLanguage.Description = language.Description;
                    databaseLanguage.IsCompilable = language.IsCompilable;
                    databaseLanguage.IsHidden = language.IsHidden;
                    databaseLanguage.Name = language.Name;
                    databaseLanguage.OutputFileTemplate = language.OutputFileTemplate;
                    databaseLanguage.RunStringFormat = language.RunStringFormat;
                }

                foreach (var language in languages.Where(o => o.Id == 0))
                {
                    var databaseLanguage = new Language
                    {
                        CompilerOptionsTemplate = language.CompilerOptionsTemplate,
                        CompilerPath = language.CompilerPath,
                        Description = language.Description,
                        IsCompilable = language.IsCompilable,
                        IsHidden = language.IsHidden,
                        Name = language.Name,
                        OutputFileTemplate = language.Name,
                        RunStringFormat = language.RunStringFormat
                    };

                    repository.Add(databaseLanguage);
                }
                uow.Commit();
            }
        }

        public IEnumerable<SubmitQueueItem> GetSubmitQueue()
        {
            using (var uow = _factory.GetUnitOfWork())
            {
                var submitResultRepository = uow.SubmitResultRepository;
                var languageRepository = uow.LanguageRepository;
                var taskRepository = uow.TaskNameRepository;
                var userRepository = uow.UserRepository;
                var contestTaskRepository = uow.ContestTaskRepository;

                var languages = languageRepository.GetLanguages().ToDictionary(o => o.Id, o => o.Name);
                var submits = submitResultRepository.GetSubmits(AllSubmitsSpecification.Instance, 1, 100).ToArray();

                var userSpecification = new UserListSpecification(submits.Select(o => o.Submit.UserId).Distinct());
                var tasks = taskRepository.GetTasks(submits.Select(o => o.Submit.ProblemId).Distinct()).ToDictionary(o => o.Id, o => o.Name);
                var users = userRepository.Find(userSpecification).ToDictionary(o => o.Id, o => o.UserName);

                var contestTasks = contestTaskRepository.GetTasks().ToArray();
                var items = submits.Select(o => GetSubmitQueueItem(o, languages, users, tasks, contestTasks));

                return items;
            }
        }

        public EditProblemViewModel GetProblem(long id)
        {
            using (var uow = _factory.GetUnitOfWork())
            {
                var taskRepository = uow.TaskRepository;
                var task = taskRepository.Get(id);
                return new EditProblemViewModel
                {
                    Id = task.Id,
                    IsOpened = task.IsOpened,
                    MemoryLimitBytes = task.MemoryLimitBytes,
                    Name = task.Name,
                    Statement = task.Statement,
                    TestsFolder = task.TestsFolder,
                    TimeLimitMilliseconds = task.TimeLimitMilliseconds
                };
            }
        }

        public long SaveProblem(EditProblemViewModel model)
        {
            using (var uow = _factory.GetUnitOfWork())
            {
                var taskRepository = uow.TaskRepository;

                Task task;
                if (model.Id != null)
                {
                    task = taskRepository.Get(model.Id.Value);
                }
                else
                {
                    task = new Task();
                    taskRepository.Add(task);
                }
                task.IsOpened = model.IsOpened;
                task.MemoryLimitBytes = model.MemoryLimitBytes;
                task.Name = model.Name;
                task.Statement = model.Statement;
                task.TestsFolder = model.TestsFolder;
                task.TimeLimitMilliseconds = model.TimeLimitMilliseconds;

                uow.Commit();
                return task.Id;
            }
        }

        public EditContestViewModel GetContest(int id)
        {
            using (var uow = _factory.GetUnitOfWork())
            {
                var contestRepository = uow.ContestsRepository;
                var contestTaskRepository = uow.ContestTaskRepository;

                var contest = contestRepository.Get(id);
                var tasks = contestTaskRepository.GetTasks(id);

                return new EditContestViewModel
                {
                    Id = contest.Id,
                    FinishTime = contest.FinishTime,
                    IsOpened = contest.IsOpened,
                    Name = contest.Name,
                    StartTime = contest.StartTime,
                    Tasks = tasks.Select(o => new TaskEditViewModel(o)).ToList()
                };
            }
        }

        public int SaveContest(EditContestViewModel model)
        {
            using (var uow = _factory.GetUnitOfWork())
            {
                var contestRepository = uow.ContestsRepository;
                var contestTaskRepository = uow.ContestTaskRepository;

                var contest = model.Id != null ? contestRepository.Get(model.Id.Value) : new Contest();
                contest.FinishTime = model.FinishTime;
                contest.IsOpened = model.IsOpened;
                contest.Name = model.Name;
                contest.StartTime = model.StartTime;

                if (model.Id == null)
                {
                    contestRepository.Add(contest);
                }

                var databaseTasks = model.Id != null
                    ? contestTaskRepository.GetTasks(model.Id.Value).ToArray()
                    : new ContestTask[0];

                foreach (var databaseTask in databaseTasks)
                {
                    var task = model.Tasks.FirstOrDefault(o => o.ProblemId == databaseTask.Task.Id);
                    if (task == null)
                    {
                        contestTaskRepository.Delete(databaseTask);
                        continue;
                    }

                    databaseTask.TaskName = task.Label;
                }

                foreach (var task in model.Tasks)
                {
                    var databaseTask = databaseTasks.FirstOrDefault(o => o.Task?.Id == task.ProblemId);
                    if (databaseTask == null)
                    {
                        databaseTask = new ContestTask
                        {
                            Contest = contest,
                            TaskId = task.ProblemId.Value
                        };
                        contestTaskRepository.Add(databaseTask);
                    }
                    databaseTask.TaskName = task.Label;
                }

                uow.Commit();
                return contest.Id;
            }
        }

        public UserListViewModel GetUsers()
        {
            using (var uow = this._factory.GetUnitOfWork())
            {
                var users = uow.UserRepository.Find(AllUsersSpecification.Instance);

                var result = new UserListViewModel
                {
                    Users = users.Select(o => new UserListItem { Id = o.Id, Email = o.Email, UserName = o.UserName }).ToArray()
                };

                return result;
            }
        }

        public UserEditViewModel GetUser(long id)
        {
            using (var uow = _factory.GetUnitOfWork())
            {
                var user = uow.UserRepository.Get(id);

                if (user == null)
                {
                    return null;
                }

                return new UserEditViewModel { Id = user.Id, Email = user.Email, UserName = user.UserName };
            }
        }

        private static SubmitQueueItem GetSubmitQueueItem
        (
            SubmitResult submitResult,
            IReadOnlyDictionary<int, string> languages,
            IReadOnlyDictionary<long, string> users,
            IReadOnlyDictionary<long, string> tasks,
            ContestTask[] contestTasks
        )
        {
            string taskLabel = null;
            int? contestId = null;

            var contestTaskSubmit = submitResult.Submit as ContestTaskSubmit;
            if (contestTaskSubmit != null)
            {
                taskLabel = contestTasks.FirstOrDefault(o => o.ContestId == contestTaskSubmit.ContestId &&
                                                 o.Task.Id == contestTaskSubmit.ProblemId)?.TaskName ?? "deleted";
                contestId = contestTaskSubmit.ContestId;
            }
            var language = languages[submitResult.Submit.LanguageId];
            var problemName = tasks[submitResult.Submit.ProblemId];
            var userName = users[submitResult.Submit.UserId];

            return new SubmitQueueItem(submitResult, language, problemName, taskLabel, contestId, userName);
        }
    }
}

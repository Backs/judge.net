using System.Collections.Generic;
using System.Linq;
using Judge.Application.Interfaces;
using Judge.Application.ViewModels.Admin.Contests;
using Judge.Application.ViewModels.Admin.Languages;
using Judge.Application.ViewModels.Admin.Problems;
using Judge.Data;
using Judge.Model.Account;
using Judge.Model.CheckSolution;
using Judge.Model.Configuration;
using Judge.Model.Contests;
using Judge.Model.Entities;
using Judge.Model.SubmitSolution;
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
            using (var uow = _factory.GetUnitOfWork(false))
            {
                var repository = uow.GetRepository<ILanguageRepository>();
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
            using (var uow = _factory.GetUnitOfWork(true))
            {
                var repository = uow.GetRepository<ILanguageRepository>();
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
            using (var uow = _factory.GetUnitOfWork(false))
            {
                var submitResultRepository = uow.GetRepository<ISubmitResultRepository>();
                var languageRepository = uow.GetRepository<ILanguageRepository>();
                var taskRepository = uow.GetRepository<ITaskNameRepository>();
                var userRepository = uow.GetRepository<IUserRepository>();
                var contestTaskRepository = uow.GetRepository<IContestTaskRepository>();

                var languages = languageRepository.GetLanguages().ToDictionary(o => o.Id, o => o.Name);
                var submits = submitResultRepository.GetSubmits(AllSubmitsSpecification.Instance, 1, 100).ToArray();

                var tasks = taskRepository.GetTasks(submits.Select(o => o.Submit.ProblemId).Distinct()).ToDictionary(o => o.Id, o => o.Name);
                var users = userRepository.GetUsers(submits.Select(o => o.Submit.UserId).Distinct()).ToDictionary(o => o.Id, o => o.UserName);

                var contestTasks = contestTaskRepository.GetTasks().ToArray();
                var items = submits.Select(o => GetSubmitQueueItem(o, languages, users, tasks, contestTasks));

                return items;
            }
        }

        public EditProblemViewModel GetProblem(long id)
        {
            using (var uow = _factory.GetUnitOfWork(false))
            {
                var taskRepository = uow.GetRepository<ITaskRepository>();
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
            using (var uow = _factory.GetUnitOfWork(true))
            {
                var taskRepository = uow.GetRepository<ITaskRepository>();

                Task task = null;
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
            using (var uow = _factory.GetUnitOfWork(false))
            {
                var contestRepository = uow.GetRepository<IContestsRepository>();
                var contestTaskRepository = uow.GetRepository<IContestTaskRepository>();

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
                taskLabel = contestTasks.First(o => o.ContestId == contestTaskSubmit.ContestId &&
                                                 o.Task.Id == contestTaskSubmit.ProblemId).TaskName;
                contestId = contestTaskSubmit.ContestId;
            }
            var language = languages[submitResult.Submit.LanguageId];
            var problemName = tasks[submitResult.Submit.ProblemId];
            var userName = users[submitResult.Submit.UserId];

            return new SubmitQueueItem(submitResult, language, problemName, taskLabel, contestId, userName);
        }
    }
}

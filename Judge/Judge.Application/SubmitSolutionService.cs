namespace Judge.Application
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Security.Authentication;
    using System.Security.Principal;
    using System.Text.RegularExpressions;
    using System.Web;
    using Judge.Application.Interfaces;
    using Judge.Application.ViewModels;
    using Judge.Application.ViewModels.Problems.Solution;
    using Judge.Application.ViewModels.Submit;
    using Judge.Data;
    using Judge.Model.Entities;
    using Judge.Model.SubmitSolution;

    internal sealed class SubmitSolutionService : ISubmitSolutionService
    {
        private static readonly Regex Regex = new Regex(@"\s+", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private readonly IUnitOfWorkFactory factory;
        private readonly IPrincipal principal;

        public SubmitSolutionService(IUnitOfWorkFactory factory, IPrincipal principal)
        {
            this.factory = factory;
            this.principal = principal;
        }

        public IEnumerable<LanguageViewModel> GetLanguages()
        {
            using (var uow = this.factory.GetUnitOfWork())
            {
                var languageRepository = uow.Languages;

                return languageRepository.GetLanguages(true).Select(o => new LanguageViewModel
                {
                    Id = o.Id,
                    Name = o.Name
                }).ToArray();
            }
        }

        public IEnumerable<LanguageViewModel> GetLanguages(int contestId, string label, long userId)
        {
            using (var uow = this.factory.GetUnitOfWork())
            {
                var languageRepository = uow.Languages;
                var contestRepository = uow.Contests;
                var submitRepository = uow.Submits;
                var contestTaskRepository = uow.ContestTasks;

                var contest = contestRepository.Get(contestId);
                var task = contestTaskRepository.Get(contestId, label);

                var languages = languageRepository.GetLanguages(true).Select(o => new LanguageViewModel
                {
                    Id = o.Id,
                    Name = o.Name
                }).ToArray();

                if (!contest.OneLanguagePerTask)
                {
                    return languages;
                }

                var submits = submitRepository.Get(new ContestUserSubmitsSpecification(userId, contestId)).ToArray();
                var submit = submits.FirstOrDefault(o => o.ProblemId == task.TaskId);

                if (submit == null)
                {
                    var usedLanguages = submits.Select(o => o.LanguageId).ToHashSet();
                    return languages.Where(o => !usedLanguages.Contains(o.Id));
                }

                return languages.Where(o => o.Id == submit.LanguageId);
            }
        }

        public void SubmitSolution(long problemId, int selectedLanguage, HttpPostedFileBase file, UserInfo userInfo)
        {
            if (file == null)
                throw new ArgumentNullException(nameof(file));

            string sourceCode;
            using (var sr = new StreamReader(file.InputStream))
            {
                sourceCode = sr.ReadToEnd();
            }

            var submit = ProblemSubmit.Create();

            submit.ProblemId = problemId;
            submit.LanguageId = selectedLanguage;
            submit.UserId = userInfo.UserId;
            submit.FileName = GetFileName(file);
            submit.SourceCode = sourceCode;
            submit.UserHost = userInfo.Host;
            submit.SessionId = userInfo.SessionId;

            using (var unitOfWork = this.factory.GetUnitOfWork())
            {
                var submitRepository = unitOfWork.Submits;
                submitRepository.Add(submit);
                unitOfWork.Commit();
            }
        }

        private static string GetFileName(HttpPostedFileBase file)
        {
            var fileName = Path.GetFileName(file.FileName);

            return Regex.Replace(fileName, "_");
        }

        public SolutionViewModel GetSolution(long submitResultId, long userId)
        {
            using (var unitOfWork = this.factory.GetUnitOfWork())
            {
                var submitResultRepository = unitOfWork.SubmitResults;
                var taskRepository = unitOfWork.Tasks;

                var result = submitResultRepository.Get(submitResultId);
                if (result == null)
                    return null;

                var hasPermission = this.principal.IsInRole("admin");

                var submit = result.Submit;

                if (!hasPermission && submit.UserId != userId)
                {
                    throw new AuthenticationException();
                }

                var problem = taskRepository.Get(submit.ProblemId);

                var totalBytes = result.TotalBytes != null ? Math.Min(result.TotalBytes.Value, problem.MemoryLimitBytes) : (int?)null;
                var totalMilliseconds = result.TotalMilliseconds != null ? Math.Min(result.TotalMilliseconds.Value, problem.TimeLimitMilliseconds) : (int?)null;

                var submitViewModel = hasPermission ? new SubmitViewModel(totalBytes, totalMilliseconds)
                {
                    PassedTests = result.PassedTests,
                    Status = result.Status,
                    CompileOutput = result.CompileOutput,
                    RunDescription = result.RunDescription,
                    RunOutput = result.RunOutput,
                    UserHost = submit.UserHost,
                    SessionId = submit.SessionId
                } : null;
                return new SolutionViewModel
                {
                    ProblemId = submit.ProblemId,
                    SourceCode = submit.SourceCode,
                    ProblemName = problem.Name,
                    SubmitResults = submitViewModel
                };
            }
        }
    }
}

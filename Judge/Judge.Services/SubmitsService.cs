using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Judge.Data;
using Judge.Model.Account;
using Judge.Model.CheckSolution;
using Judge.Model.Configuration;
using Judge.Model.Entities;
using Judge.Model.SubmitSolution;
using Judge.Services.Converters;
using Judge.Web.Client.Submits;
using Task = System.Threading.Tasks.Task;

namespace Judge.Services;

internal sealed class SubmitsService : ISubmitsService
{
    private static readonly Regex WhitespaceRegex = new Regex(@"\s+", RegexOptions.Compiled | RegexOptions.IgnoreCase);
    private readonly IUnitOfWorkFactory unitOfWorkFactory;

    public SubmitsService(IUnitOfWorkFactory unitOfWorkFactory)
    {
        this.unitOfWorkFactory = unitOfWorkFactory;
    }

    public async Task<SubmitsList> SearchAsync(SubmitsQuery query)
    {
        await using var unitOfWork = this.unitOfWorkFactory.GetUnitOfWork();

        var submitResultRepository = unitOfWork.SubmitResultRepository;
        var languageRepository = unitOfWork.LanguageRepository;
        var taskRepository = unitOfWork.TaskRepository;
        var userRepository = unitOfWork.UserRepository;

        var languages = await languageRepository.GetDictionaryAsync(false);
        var specification = new AdminSearchSubmitsSpecification(null, null);
        var submits =
            await submitResultRepository.SearchAsync(specification, query.Skip, query.Take);

        var userSpecification = new UserListSpecification(submits.Select(o => o.Submit.UserId).Distinct());
        var tasks = await taskRepository.GetDictionaryAsync(submits.Select(o => o.Submit.ProblemId).Distinct());
        var users = await userRepository.GetDictionaryAsync(userSpecification);

        var items = submits.Select(o =>
                SubmitsConverter.Convert(o, languages[o.Submit.LanguageId], tasks[o.Submit.ProblemId],
                    users[o.Submit.UserId]))
            .ToArray();

        var totalCount = await submitResultRepository.CountAsync(specification);
        return new SubmitsList
        {
            Items = items,
            TotalCount = totalCount
        };
    }

    public async Task SaveAsync(SubmitSolution submitSolution, SubmitUserInfo userInfo)
    {
        if (submitSolution.ContestId != null)
        {
            await this.SaveContestSubmitAsync(submitSolution, userInfo);
        }
        else
        {
            await this.SaveSubmitAsync(submitSolution, userInfo);
        }
    }

    private async Task SaveSubmitAsync(SubmitSolution submitSolution, SubmitUserInfo userInfo)
    {
        using var sr = new StreamReader(submitSolution.File.OpenReadStream());
        var sourceCode = await sr.ReadToEndAsync();

        var submit = ProblemSubmit.Create();

        submit.ProblemId = submitSolution.ProblemId!.Value;
        submit.LanguageId = submitSolution.LanguageId;
        submit.UserId = userInfo.UserId;
        submit.FileName = GetFileName(submitSolution.File.FileName);
        submit.SourceCode = sourceCode;
        submit.UserHost = userInfo.Host;

        await using var unitOfWork = this.unitOfWorkFactory.GetUnitOfWork();
        unitOfWork.SubmitRepository.Add(submit);
        await unitOfWork.CommitAsync();
    }

    private async Task SaveContestSubmitAsync(SubmitSolution submitSolution, SubmitUserInfo userInfo)
    {
        await using var unitOfWork = this.unitOfWorkFactory.GetUnitOfWork();
        var contestId = submitSolution.ContestId!.Value;
        var contest = await unitOfWork.ContestsRepository.TryGetAsync(contestId);

        if (contest == null)
            throw new InvalidOperationException("Contest not found");

        if (DateTime.UtcNow < contest.StartTime)
            throw new InvalidOperationException("Contest not started");

        if (DateTime.UtcNow >= contest.FinishTime)
            throw new InvalidOperationException("Contest finished");

        var task = await unitOfWork.ContestTaskRepository.TryGetAsync(contestId, submitSolution.TaskLabel);

        if (task == null)
            throw new InvalidOperationException("Task not found");

        if (!contest.OneLanguagePerTask)
        {
            return;
        }

        var availableLanguages = (await unitOfWork.LanguageRepository.GetAllAsync(true)).Select(o => o.Id).ToHashSet();

        var submits =
            await unitOfWork.SubmitRepository.SearchAsync(
                new ContestUserSubmitsSpecification(userInfo.UserId, contestId));
        var prevSubmit = submits.FirstOrDefault(o => o.ProblemId == task.TaskId);

        if (prevSubmit == null)
        {
            var usedLanguages = submits.Select(o => o.LanguageId).ToHashSet();
            availableLanguages.ExceptWith(usedLanguages);
        }
        else
        {
            availableLanguages = availableLanguages.Where(o => o == prevSubmit.LanguageId).ToHashSet();
        }

        if (!availableLanguages.Contains(submitSolution.LanguageId))
        {
            throw new InvalidOperationException("Language not available");
        }

        using var sr = new StreamReader(submitSolution.File.OpenReadStream());
        var sourceCode = await sr.ReadToEndAsync();

        var submit = ContestTaskSubmit.Create();

        submit.ProblemId = task.Task.Id;
        submit.ContestId = contestId;
        submit.LanguageId = submitSolution.LanguageId;
        submit.UserId = userInfo.UserId;
        submit.FileName = GetFileName(submitSolution.File.FileName);
        submit.SourceCode = sourceCode;
        submit.UserHost = userInfo.Host;

        unitOfWork.SubmitRepository.Add(submit);
        await unitOfWork.CommitAsync();
    }

    private static string GetFileName(string fileName)
    {
        fileName = Path.GetFileName(fileName);

        return WhitespaceRegex.Replace(fileName, "_");
    }
}
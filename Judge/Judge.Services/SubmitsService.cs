using System;
using System.Linq;
using System.Threading.Tasks;
using Judge.Data;
using Judge.Model.Account;
using Judge.Model.CheckSolution;
using Judge.Model.Configuration;
using Judge.Model.Contests;
using Judge.Model.Entities;
using Judge.Model.SubmitSolution;
using Judge.Services.Converters;
using Client = Judge.Web.Client;
using SubmitsQuery = Judge.Services.Model.SubmitsQuery;

namespace Judge.Services;

internal sealed class SubmitsService : ISubmitsService
{
    private readonly IUnitOfWorkFactory unitOfWorkFactory;
    private readonly ISubmitsConverter submitsConverter;
    private readonly IFileNameResolver fileNameResolver;

    public SubmitsService(IUnitOfWorkFactory unitOfWorkFactory, ISubmitsConverter submitsConverter,
        IFileNameResolver fileNameResolver)
    {
        this.unitOfWorkFactory = unitOfWorkFactory;
        this.submitsConverter = submitsConverter;
        this.fileNameResolver = fileNameResolver;
    }

    public async Task<Client.Submits.SubmitResultsList> SearchAsync(SubmitsQuery query, bool openedOnly)
    {
        await using var unitOfWork = this.unitOfWorkFactory.GetUnitOfWork();

        if (query is { ContestId: not null, TaskLabel: not null, ProblemId: null })
        {
            var contestTask =
                await unitOfWork.ContestTasks.TryGetAsync(query.ContestId.Value, query.TaskLabel);
            query.ProblemId = contestTask?.TaskId;
            openedOnly = false;
        }

        var languages = await unitOfWork.Languages.GetDictionaryAsync(false);
        var specification = new SubmitsSpecification(
            type: Convert(query.Type),
            contestId: query.ContestId,
            problemId: query.ProblemId,
            userId: query.UserId,
            openedOnly: openedOnly);

        var submitResults = await unitOfWork.SubmitResults.SearchAsync(specification, query.Skip, query.Take);

        var userSpecification = new UserListSpecification(submitResults.Select(o => o.Submit.UserId).Distinct());
        var tasks = await unitOfWork.Tasks.GetDictionaryAsync(submitResults.Select(o => o.Submit.ProblemId)
            .Distinct());
        var users = await unitOfWork.Users.GetDictionaryAsync(userSpecification);

        var contestIds = submitResults.Select(o => o.Submit).OfType<ContestTaskSubmit>().Select(o => o.ContestId)
            .Distinct();

        var contestTasks = await unitOfWork.ContestTasks.SearchAsync(contestIds);

        var items = submitResults.Select(o => this.submitsConverter.Convert<Client.Submits.SubmitResultInfo>(o,
                languages[o.Submit.LanguageId],
                tasks[o.Submit.ProblemId],
                users[o.Submit.UserId], contestTasks))
            .ToArray();

        var totalCount = await unitOfWork.SubmitResults.CountAsync(specification);
        return new Client.Submits.SubmitResultsList
        {
            Items = items,
            TotalCount = totalCount
        };
    }

    private static SubmitsType Convert(Model.SubmitsType type) =>
        type switch
        {
            Model.SubmitsType.All => SubmitsType.All,
            Model.SubmitsType.Problem => SubmitsType.Problem,
            Model.SubmitsType.Contest => SubmitsType.Contest,
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };

    public async Task<long> SaveAsync(Client.Submits.SubmitSolution submitSolution,
        Client.Submits.SubmitUserInfo userInfo)
    {
        if (submitSolution.ContestId != null)
        {
            return await this.SaveContestSubmitAsync(submitSolution, userInfo);
        }

        return await this.SaveSubmitAsync(submitSolution, userInfo);
    }

    public async Task<Client.Submits.SubmitResultExtendedInfo?> GetResultAsync(long id)
    {
        await using var unitOfWork = this.unitOfWorkFactory.GetUnitOfWork();
        var submitResult = await unitOfWork.SubmitResults.GetAsync(id);

        if (submitResult == null)
            return null;

        var languages = await unitOfWork.Languages.GetDictionaryAsync(false);
        var user = await unitOfWork.Users.GetAsync(submitResult.Submit.UserId);
        var task = await unitOfWork.Tasks.GetAsync(submitResult.Submit.ProblemId);

        var contestTasks = submitResult.Submit switch
        {
            ContestTaskSubmit contestTaskSubmit => await unitOfWork.ContestTasks.SearchAsync(
                contestTaskSubmit.ContestId),
            _ => Array.Empty<ContestTask>()
        };

        var result = this.submitsConverter.Convert<Client.Submits.SubmitResultExtendedInfo>(submitResult,
            languages[submitResult.Submit.LanguageId],
            task!,
            user!,
            contestTasks);

        result.SourceCode = submitResult.Submit.SourceCode;
        result.CompilerOutput = submitResult.CompileOutput;
        result.RunOutput = submitResult.RunOutput;
        result.UserHost = submitResult.Submit.UserHost;

        return result;
    }

    public async Task<Client.Submits.SubmitResultExtendedInfo?> RejudgeAsync(long id)
    {
        await using var unitOfWork = this.unitOfWorkFactory.GetUnitOfWork();
        var submitResult = await unitOfWork.SubmitResults.GetAsync(id);

        if (submitResult == null)
            return null;

        var newSubmitResult = new SubmitResult(submitResult.Submit);
        newSubmitResult = await unitOfWork.SubmitResults.SaveAsync(newSubmitResult);

        await unitOfWork.CommitAsync();

        return await this.GetResultAsync(newSubmitResult.Id);
    }

    private async Task<long> SaveSubmitAsync(Client.Submits.SubmitSolution submitSolution,
        Client.Submits.SubmitUserInfo userInfo)
    {
        await using var unitOfWork = this.unitOfWorkFactory.GetUnitOfWork();
        var allLanguages = await unitOfWork.Languages.GetAllAsync(true);

        var submit = ProblemSubmit.Create();

        submit.ProblemId = submitSolution.ProblemId!.Value;
        submit.LanguageId = submitSolution.LanguageId;
        submit.UserId = userInfo.UserId;
        submit.FileName = this.fileNameResolver.Resolve(submitSolution.Solution, submit.LanguageId, allLanguages);
        submit.SourceCode = submitSolution.Solution;
        submit.UserHost = userInfo.Host;

        unitOfWork.Submits.Add(submit);
        await unitOfWork.CommitAsync();

        return submit.Id;
    }

    private async Task<long> SaveContestSubmitAsync(Client.Submits.SubmitSolution submitSolution,
        Client.Submits.SubmitUserInfo userInfo)
    {
        await using var unitOfWork = this.unitOfWorkFactory.GetUnitOfWork();
        var contestId = submitSolution.ContestId!.Value;
        var contest = await unitOfWork.Contests.TryGetAsync(contestId);

        if (contest == null)
            throw new InvalidOperationException("Contest not found");

        if (DateTime.UtcNow < contest.StartTime)
            throw new InvalidOperationException("Contest not started");

        if (DateTime.UtcNow >= contest.FinishTime)
            throw new InvalidOperationException("Contest finished");

        var task = await unitOfWork.ContestTasks.TryGetAsync(contestId, submitSolution.ProblemLabel);

        if (task == null)
            throw new InvalidOperationException("Task not found");

        var allLanguages = await unitOfWork.Languages.GetAllAsync(true);

        if (contest.OneLanguagePerTask)
        {
            var availableLanguages = allLanguages.Select(o => o.Id).ToHashSet();

            var submits =
                await unitOfWork.Submits.SearchAsync(
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
        }

        var submit = ContestTaskSubmit.Create();

        submit.ProblemId = task.Task.Id;
        submit.ContestId = contestId;
        submit.LanguageId = submitSolution.LanguageId;
        submit.UserId = userInfo.UserId;
        submit.FileName = this.fileNameResolver.Resolve(submitSolution.Solution, submit.LanguageId, allLanguages);
        submit.SourceCode = submitSolution.Solution;
        submit.UserHost = userInfo.Host;

        unitOfWork.Submits.Add(submit);
        await unitOfWork.CommitAsync();
        return submit.Id;
    }
}
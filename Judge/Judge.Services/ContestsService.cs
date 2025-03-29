using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Judge.Data;
using Judge.Model;
using Judge.Model.Account;
using Judge.Model.Contests;
using Judge.Model.Entities;
using Judge.Services.Converters.Contests;
using Judge.Services.Model;
using Judge.Web.Client.Problems;
using Client = Judge.Web.Client.Contests;

namespace Judge.Services;

internal sealed class ContestsService : IContestsService
{
    private readonly IUnitOfWorkFactory unitOfWorkFactory;
    private readonly IContestConverterFactory contestConverterFactory;

    public ContestsService(IUnitOfWorkFactory unitOfWorkFactory, IContestConverterFactory contestConverterFactory)
    {
        this.unitOfWorkFactory = unitOfWorkFactory;
        this.contestConverterFactory = contestConverterFactory;
    }

    public async Task<Client.ContestsInfoList> SearchAsync(Client.ContestsQuery query, bool openedOnly)
    {
        await using var unitOfWork = this.unitOfWorkFactory.GetUnitOfWork();
        ISpecification<Contest> specification = query.UpcomingOnly
            ? new UpcomingContestSpecification(DateTime.UtcNow)
            : new AllContestsSpecification(openedOnly);

        var contests = await unitOfWork.Contests.SearchAsync(specification, query.Skip, query.Take);
        var totalCount = await unitOfWork.Contests.CountAsync(specification);

        var items = contests.Select(Convert<Client.ContestInfo>).ToArray();

        return new Client.ContestsInfoList
        {
            Items = items,
            TotalCount = totalCount
        };
    }

    public async Task<Client.Contest?> GetAsync(int id, long? userId)
    {
        await using var unitOfWork = this.unitOfWorkFactory.GetUnitOfWork();
        var contest = await unitOfWork.Contests.TryGetAsync(id);
        if (contest == null)
            return null;

        var tasks = await unitOfWork.ContestTasks.SearchAsync(id);

        var solvedTasks = new HashSet<long>();

        if (userId != null)
        {
            var specification =
                new UserContestSolvedProblemsSpecification(id, userId.Value, tasks.Select(o => o.Task.Id));

            var submitResults = await unitOfWork.SubmitResults.SearchAsync(specification, 0, int.MaxValue);

            solvedTasks.UnionWith(submitResults.Select(o => o.Submit.ProblemId));
        }

        var result = Convert<Client.Contest>(contest);
        result.Tasks = tasks.Select(o => new Client.ContestTask
        {
            Name = o.Task.Name,
            Label = o.TaskName,
            Solved = solvedTasks.Contains(o.Task.Id)
        }).ToArray();

        return result;
    }

    public async Task<Client.EditContest?> GetEditableAsync(int id)
    {
        await using var unitOfWork = this.unitOfWorkFactory.GetUnitOfWork();
        var contest = await unitOfWork.Contests.TryGetAsync(id);
        if (contest == null)
            return null;

        var tasks = await unitOfWork.ContestTasks.SearchAsync(id);


        var result = new Client.EditContest
        {
            Name = contest.Name,
            Id = contest.Id,
            StartTime = contest.StartTime.SetUtcKind(),
            FinishTime = contest.FinishTime.SetUtcKind(),
            Rules = GetRules(contest.Rules),
            Problems = tasks.Select(o => new Client.EditContestProblem
            {
                ProblemId = o.TaskId,
                Label = o.TaskName,
                Name = o.Task.Name
            }).ToArray(),
            IsOpened = contest.IsOpened,
            CheckPointTime = contest.CheckPointTime?.SetUtcKind(),
            OneLanguagePerTask = contest.OneLanguagePerTask
        };

        return result;
    }

    public async Task<Client.ContestResult?> GetResultAsync(int id)
    {
        await using var unitOfWork = this.unitOfWorkFactory.GetUnitOfWork();
        var contest = await unitOfWork.Contests.TryGetAsync(id);
        if (contest == null)
            return null;

        var contestTasks = await GetContestTasksAsync(id, unitOfWork);
        var result = Convert<Client.ContestResult>(contest);
        result.Rules = Convert(contest.Rules);

        var contestResults = await unitOfWork.ContestTasks.GetResultsAsync(id);

        var users = await GetUsersAsync(unitOfWork, contestResults);

        var converter = this.contestConverterFactory.Get(contest.Rules);
        result.Users = converter.Convert(contest, contestTasks, contestResults, users);

        result.Tasks = contestTasks.Values.Select(o => new Client.ContestTask
        {
            Name = o.Task.Name,
            Label = o.TaskName
        }).ToArray();

        return result;
    }

    public async Task<Client.EditContest?> SaveAsync(Client.EditContest editContest)
    {
        await using var unitOfWork = this.unitOfWorkFactory.GetUnitOfWork();

        Contest contest;
        if (editContest.Id != null)
        {
            contest = await unitOfWork.Contests.TryGetAsync(editContest.Id.Value);
            if (contest == null)
                return null;
        }
        else
        {
            contest = new Contest();
        }

        contest.Name = editContest.Name;
        contest.StartTime = editContest.StartTime;
        contest.FinishTime = editContest.FinishTime;
        contest.CheckPointTime = editContest.CheckPointTime;
        contest.OneLanguagePerTask = editContest.OneLanguagePerTask;
        contest.IsOpened = editContest.IsOpened;
        contest.Rules = Convert(editContest.Rules);

        if (editContest.Id == null)
            unitOfWork.Contests.Add(contest);

        var databaseTasks = editContest.Id != null
            ? await unitOfWork.ContestTasks.SearchAsync(editContest.Id.Value)
            : Array.Empty<ContestTask>();

        foreach (var databaseTask in databaseTasks)
        {
            var task = editContest.Problems.FirstOrDefault(o => o.ProblemId == databaseTask.Task.Id);
            if (task == null)
            {
                unitOfWork.ContestTasks.Delete(databaseTask);
                continue;
            }

            databaseTask.TaskName = task.Label;
        }

        foreach (var task in editContest.Problems)
        {
            var databaseTask = databaseTasks.FirstOrDefault(o => o.Task?.Id == task.ProblemId);
            if (databaseTask == null)
            {
                databaseTask = new ContestTask
                {
                    Contest = contest,
                    TaskId = task.ProblemId,
                    TaskName = task.Label
                };
                unitOfWork.ContestTasks.Add(databaseTask);
            }

            databaseTask.TaskName = task.Label;
        }

        await unitOfWork.CommitAsync();

        editContest.Id = contest.Id;

        return editContest;
    }

    public async Task<Problem?> GetProblemAsync(int contestId, string label, long? userId)
    {
        await using var unitOfWork = this.unitOfWorkFactory.GetUnitOfWork();
        var contest = await unitOfWork.Contests.TryGetAsync(contestId);
        if (contest == null)
        {
            return null;
        }

        if (!contest.IsOpened)
        {
            return null;
        }

        if (contest.StartTime > DateTime.UtcNow)
        {
            return null;
        }

        var problem = await unitOfWork.ContestTasks.TryGetAsync(contestId, label);

        var allLanguages = await unitOfWork.Languages.GetAllAsync(true);

        if (contest.OneLanguagePerTask && userId != null)
        {
            var languages = (await unitOfWork.Languages.GetAllAsync(true)).Select(o => o.Id).ToHashSet();

            var submits =
                await unitOfWork.Submits.SearchAsync(
                    new ContestUserSubmitsSpecification(userId.Value, contestId));
            var prevSubmit = submits.FirstOrDefault(o => o.ProblemId == problem.TaskId);

            if (prevSubmit == null)
            {
                var usedLanguages = submits.Select(o => o.LanguageId).ToHashSet();
                languages.ExceptWith(usedLanguages);
            }
            else
            {
                languages = languages.Where(o => o == prevSubmit.LanguageId).ToHashSet();
            }

            allLanguages = allLanguages.Where(c => languages.Contains(c.Id)).ToHashSet();
        }

        if (problem == null)
        {
            return null;
        }

        var task = problem.Task;

        return new Problem
        {
            Id = task.Id,
            Name = task.Name,
            Statement = task.Statement,
            MemoryLimitBytes = task.MemoryLimitBytes,
            TimeLimitMilliseconds = task.TimeLimitMilliseconds,
            Languages = allLanguages.Select(o => new ProblemLanguage
            {
                Id = o.Id,
                Name = o.Name
            }).ToArray()
        };
    }

    private static ContestRules Convert(Client.ContestRules rules) =>
        rules switch
        {
            Client.ContestRules.Acm => ContestRules.Acm,
            Client.ContestRules.Points => ContestRules.Points,
            Client.ContestRules.CheckPoint => ContestRules.CheckPoint,
            _ => throw new ArgumentOutOfRangeException(nameof(rules), rules, null)
        };

    private static async Task<IReadOnlyDictionary<long, ContestTask>> GetContestTasksAsync(int id,
        IUnitOfWork unitOfWork)
    {
        var contestTasks = await unitOfWork.ContestTasks.SearchAsync(id);
        return contestTasks.ToDictionary(o => o.TaskId);
    }

    private static async Task<IReadOnlyDictionary<long, User>> GetUsersAsync(IUnitOfWork unitOfWork,
        IReadOnlyCollection<ContestResult> contestResults)
    {
        var users = await unitOfWork.Users.SearchAsync(
            new UserListSpecification(contestResults.Select(o => o.UserId).Distinct()));
        return users.ToDictionary(o => o.Id);
    }

    private static Client.ContestRules Convert(ContestRules contestRules) =>
        contestRules switch
        {
            ContestRules.Acm => Client.ContestRules.Acm,
            ContestRules.Points => Client.ContestRules.Points,
            ContestRules.CheckPoint => Client.ContestRules.CheckPoint,
            _ => throw new ArgumentOutOfRangeException(nameof(contestRules), contestRules, null)
        };

    private static T Convert<T>(Contest contest)
        where T : Client.ContestInfo, new() =>
        new()
        {
            Id = contest.Id,
            Name = contest.Name,
            StartDate = contest.StartTime,
            EndDate = contest.FinishTime,
            Duration = (contest.FinishTime - contest.StartTime),
            Status = GetStatus(contest),
            Rules = GetRules(contest.Rules),
            IsOpened = contest.IsOpened
        };

    private static Client.ContestRules GetRules(ContestRules contestRules) =>
        contestRules switch
        {
            ContestRules.Acm => Client.ContestRules.Acm,
            ContestRules.Points => Client.ContestRules.Points,
            ContestRules.CheckPoint => Client.ContestRules.CheckPoint,
            _ => throw new ArgumentOutOfRangeException(nameof(contestRules), contestRules, null)
        };

    private static Client.ContestStatus GetStatus(Contest contest)
    {
        if (contest.StartTime > DateTime.UtcNow)
            return Client.ContestStatus.Planned;

        if (contest.StartTime <= DateTime.UtcNow && contest.FinishTime > DateTime.UtcNow)
            return Client.ContestStatus.Running;

        return Client.ContestStatus.Completed;
    }
}
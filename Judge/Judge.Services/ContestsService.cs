﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Judge.Data;
using Judge.Model.Account;
using Judge.Model.Contests;
using Judge.Model.Entities;
using Judge.Services.Converters.Contests;
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

    public async Task<Client.ContestsInfoList> SearchAsync(Client.ContestsQuery query)
    {
        await using var unitOfWork = this.unitOfWorkFactory.GetUnitOfWork();
        var specification = AllContestsSpecification.Instance;
        var contests = await unitOfWork.Contests.SearchAsync(specification, query.Skip, query.Take);

        var items = contests.Select(Convert<Client.ContestInfo>).ToArray();

        return new Client.ContestsInfoList { Items = items };
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

        return result;
    }

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
            Name = contest.Name,
            StartDate = contest.StartTime,
            Duration = contest.FinishTime - contest.StartTime,
            Status = GetStatus(contest)
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
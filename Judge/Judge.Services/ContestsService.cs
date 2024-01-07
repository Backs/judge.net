using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Judge.Data;
using Judge.Model.Contests;
using Client = Judge.Web.Client.Contests;

namespace Judge.Services;

internal sealed class ContestsService : IContestsService
{
    private readonly IUnitOfWorkFactory unitOfWorkFactory;

    public ContestsService(IUnitOfWorkFactory unitOfWorkFactory)
    {
        this.unitOfWorkFactory = unitOfWorkFactory;
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

    private static T Convert<T>(Contest contest)
        where T : Client.ContestInfo, new()
    {
        return new T
        {
            Name = contest.Name,
            StartDate = contest.StartTime,
            Duration = contest.FinishTime - contest.StartTime,
            Status = GetStatus(contest)
        };
    }

    private static Client.ContestStatus GetStatus(Contest contest)
    {
        if (contest.StartTime > DateTime.UtcNow)
            return Client.ContestStatus.Planned;

        if (contest.StartTime <= DateTime.UtcNow && contest.FinishTime > DateTime.UtcNow)
            return Client.ContestStatus.Running;

        return Client.ContestStatus.Completed;
    }
}
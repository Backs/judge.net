using System;
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

    public async Task<Client.ContestsList> SearchAsync(Client.ContestsQuery query)
    {
        await using var unitOfWork = this.unitOfWorkFactory.GetUnitOfWork();
        var specification = AllContestsSpecification.Instance;
        var contests = await unitOfWork.ContestsRepository.SearchAsync(specification, query.Skip, query.Take);

        var items = contests.Select(o => new Client.Contest
        {
            Name = o.Name,
            StartDate = o.StartTime,
            Duration = o.FinishTime - o.StartTime,
            Status = GetStatus(o)
        }).ToArray();

        return new Client.ContestsList { Items = items };
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
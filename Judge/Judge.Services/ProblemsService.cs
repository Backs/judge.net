using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Judge.Data;
using Judge.Model.CheckSolution;
using Judge.Model.SubmitSolution;
using Judge.Web.Client.Problems;

namespace Judge.Services;

internal sealed class ProblemsService : IProblemsService
{
    private readonly IUnitOfWorkFactory unitOfWorkFactory;

    public ProblemsService(IUnitOfWorkFactory unitOfWorkFactory)
    {
        this.unitOfWorkFactory = unitOfWorkFactory;
    }

    public async Task<ProblemsList> SearchAsync(long? userId, ProblemsQuery query)
    {
        await using var unitOfWork = this.unitOfWorkFactory.GetUnitOfWork(false);
        var tasks = await unitOfWork.Tasks.GetTasksAsync(OpenedTasksSpecification.Instance, query.Skip,
            query.Take);

        var totalCount = await unitOfWork.Tasks.CountAsync(OpenedTasksSpecification.Instance);

        var solved = new HashSet<long>();

        if (userId != null)
        {
            var userSolved = await unitOfWork.SubmitResults.GetSolvedProblemsAsync(
                new UserSolvedProblemsSpecification(userId.Value, tasks.Select(o => o.Id).ToImmutableHashSet()));
            solved.UnionWith(userSolved);
        }

        var problems = tasks.Select(o => new ProblemInfo
        {
            Id = o.Id,
            Name = o.Name,
            Solved = solved.Contains(o.Id)
        }).ToArray();

        return new ProblemsList
        {
            Items = problems,
            TotalCount = totalCount
        };
    }

    public async Task<Problem?> GetAsync(long id)
    {
        await using var unitOfWork = this.unitOfWorkFactory.GetUnitOfWork(false);
        var task = await unitOfWork.Tasks.GetAsync(id);

        if (task == null)
        {
            return null;
        }

        return new Problem
        {
            Id = task.Id,
            Name = task.Name,
            Statement = task.Statement,
            MemoryLimitBytes = task.MemoryLimitBytes,
            TimeLimitMilliseconds = task.TimeLimitMilliseconds
        };
    }
}
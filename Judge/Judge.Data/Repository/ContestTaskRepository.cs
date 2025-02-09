using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Judge.Model.Contests;
using Judge.Model.SubmitSolution;
using Microsoft.EntityFrameworkCore;

namespace Judge.Data.Repository;

internal sealed class ContestTaskRepository : IContestTaskRepository
{
    private readonly DataContext context;

    public ContestTaskRepository(DataContext context)
    {
        this.context = context;
    }

    public void Add(ContestTask task)
    {
        this.context.Set<ContestTask>().Add(task);
    }

    public void Delete(ContestTask task)
    {
        this.context.Set<ContestTask>().Remove(task);
    }

    public async Task<IReadOnlyCollection<ContestTask>> SearchAsync(IEnumerable<int> contestIds)
    {
        return await this.context.Set<ContestTask>()
            .Where(o => contestIds.Contains(o.Contest.Id))
            .ToListAsync();
    }

    public ContestTask? Get(int contestId, string label)
    {
        return this.context.Set<ContestTask>()
            .Include(o => o.Task)
            .Include(o => o.Contest)
            .FirstOrDefault(o => o.Contest.Id == contestId && o.TaskName == label);
    }

    public Task<ContestTask?> TryGetAsync(int contestId, string label)
    {
        return this.context.Set<ContestTask>()
            .Include(o => o.Task)
            .Include(o => o.Contest)
            .FirstOrDefaultAsync(o => o.Contest.Id == contestId && o.TaskName == label)!;
    }

    public async Task<IReadOnlyCollection<ContestTask>> SearchAsync(int contestId)
    {
        return await this.context.Set<ContestTask>()
            .Include(o => o.Task)
            .Where(o => o.ContestId == contestId)
            .OrderBy(o => o.TaskName)
            .ToListAsync();
    }

    public async Task<IReadOnlyCollection<ContestResult>> GetResultsAsync(long contestId)
    {
        var contestTasks = await this.context.Set<ContestTask>().Where(o => o.ContestId == contestId)
            .Select(o => o.TaskId)
            .ToArrayAsync();

        var plainResults = await this.context.Set<ContestTaskSubmit>()
            .Where(o => o.ContestId == contestId && contestTasks.Contains(o.ProblemId))
            .Select(o => new
            {
                o.UserId,
                o.ProblemId,
                Results = o.Results.Select(s => new
                {
                    s.Id,
                    s.Status,
                    s.Submit.SubmitDateUtc
                })
            })
            .ToArrayAsync();

        var result = plainResults
            .GroupBy(o => o.UserId)
            .Select(o => new
            {
                UserId = o.Key,
                TaskResults = o.GroupBy(p => p.ProblemId).Select(p => new
                {
                    ProblemId = p.Key,
                    SubmitResults = p
                        .Select(s => s.Results.OrderByDescending(t => t.Id).FirstOrDefault()) //only last judge result
                        .Where(s => s != null && s.Status != SubmitStatus.ServerError &&
                                    s.Status != SubmitStatus.Pending && s.Status != SubmitStatus.CompilationError)
                        .Select(s => new
                        {
                            s!.Id,
                            s.Status,
                            s.SubmitDateUtc
                        }).OrderBy(s => s.Id).ToArray()
                })
            })
            .Select(o => new
            {
                UserId = o.UserId,
                TaskResults = o.TaskResults.Select(t => new
                {
                    Solved = t.SubmitResults.Any(s => s.Status == SubmitStatus.Accepted),
                    ProblemId = t.ProblemId,
                    SubmitResults = t.SubmitResults,
                    FirstSuccess = t.SubmitResults.FirstOrDefault(s => s.Status == SubmitStatus.Accepted)
                })
            })
            .Select(o => new ContestResult
            {
                UserId = o.UserId,
                TaskResults = o.TaskResults.Where(t => t.SubmitResults.Any()).Select(t => new ContestTaskResult
                {
                    Solved = t.Solved,
                    ProblemId = t.ProblemId,
                    Attempts = t.FirstSuccess == null
                        ? t.SubmitResults.Length
                        : t.SubmitResults.Count(s => s.Id <= t.FirstSuccess.Id),
                    SubmitDateUtc = t.FirstSuccess?.SubmitDateUtc ?? t.SubmitResults.Last().SubmitDateUtc
                }).ToArray()
            })
            .Where(t => t.TaskResults.Any());
        return result.ToArray();
    }
}
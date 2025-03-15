using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Judge.Data;
using Judge.Model.CheckSolution;
using Judge.Model.SubmitSolution;
using Client = Judge.Web.Client.Problems;
using Admin = Judge.Web.Client.Admin;
using Task = Judge.Model.CheckSolution.Task;

namespace Judge.Services;

internal sealed class ProblemsService : IProblemsService
{
    private readonly IUnitOfWorkFactory unitOfWorkFactory;

    public ProblemsService(IUnitOfWorkFactory unitOfWorkFactory)
    {
        this.unitOfWorkFactory = unitOfWorkFactory;
    }

    public async Task<Client.ProblemsList> SearchAsync(long? userId, Client.ProblemsQuery query, bool openedOnly)
    {
        await using var unitOfWork = this.unitOfWorkFactory.GetUnitOfWork(false);
        var specification = new TasksSpecification(query.Name, openedOnly);
        var tasks = await unitOfWork.Tasks.GetTasksAsync(specification, query.Skip,
            query.Take);

        var totalCount = await unitOfWork.Tasks.CountAsync(specification);

        var solved = new HashSet<long>();

        if (userId != null)
        {
            var userSolved = await unitOfWork.SubmitResults.GetSolvedProblemsAsync(
                new UserSolvedProblemsSpecification(userId.Value, tasks.Select(o => o.Id).ToImmutableHashSet()));
            solved.UnionWith(userSolved);
        }

        var problems = tasks.Select(o => new Client.ProblemInfo
        {
            Id = o.Id,
            Name = o.Name,
            Solved = solved.Contains(o.Id),
            IsOpened  = o.IsOpened,
        }).ToArray();

        return new Client.ProblemsList
        {
            Items = problems,
            TotalCount = totalCount
        };
    }

    public async Task<Admin.AllProblemsList> GetAllAsync(int skip, int take)
    {
        await using var unitOfWork = this.unitOfWorkFactory.GetUnitOfWork(false);
        var specification = AllTasksSpecification.Instance;
        var tasks = await unitOfWork.Tasks.GetTasksAsync(specification, skip, take);

        var totalCount = await unitOfWork.Tasks.CountAsync(specification);

        var problems = tasks.Select(o => new Admin.AllProblemInfo
        {
            Id = o.Id,
            Name = o.Name,
            IsOpened = o.IsOpened
        }).ToArray();

        return new Admin.AllProblemsList
        {
            Items = problems,
            TotalCount = totalCount
        };
    }

    public async Task<Client.Problem?> GetAsync(long id)
    {
        await using var unitOfWork = this.unitOfWorkFactory.GetUnitOfWork(false);
        var task = await unitOfWork.Tasks.GetAsync(id);
        var languages = await unitOfWork.Languages.GetAllAsync(true);

        if (task == null)
        {
            return null;
        }

        return new Client.Problem
        {
            Id = task.Id,
            Name = task.Name,
            Statement = task.Statement,
            MemoryLimitBytes = task.MemoryLimitBytes,
            TimeLimitMilliseconds = task.TimeLimitMilliseconds,
            IsOpened = task.IsOpened,
            Languages = languages.Select(o => new Client.ProblemLanguage
            {
                Id = o.Id,
                Name = o.Name
            }).ToArray()
        };
    }

    public async Task<Client.EditProblem?> GetEditableAsync(long id)
    {
        await using var unitOfWork = this.unitOfWorkFactory.GetUnitOfWork(false);

        var task = await unitOfWork.Tasks.GetAsync(id);
        if (task == null)
            return null;

        return new Client.EditProblem
        {
            Id = task.Id,
            Name = task.Name,
            Statement = task.Statement,
            MemoryLimitBytes = task.MemoryLimitBytes,
            TestsFolder = task.TestsFolder,
            IsOpened = task.IsOpened,
            TimeLimitMilliseconds = task.TimeLimitMilliseconds
        };
    }

    public async Task<Client.EditProblem?> SaveAsync(Client.EditProblem problem)
    {
        await using var unitOfWork = this.unitOfWorkFactory.GetUnitOfWork(false);
        Task? task;
        if (problem.Id != null)
        {
            task = await unitOfWork.Tasks.GetAsync(problem.Id.Value);
            if (task == null)
                return null;
        }
        else
        {
            task = new Task();
        }

        task.Name = problem.Name;
        task.Statement = problem.Statement;
        task.MemoryLimitBytes = problem.MemoryLimitBytes;
        task.TimeLimitMilliseconds = problem.TimeLimitMilliseconds;
        task.IsOpened = problem.IsOpened;
        task.TestsFolder = problem.TestsFolder;

        if (problem.Id == null)
            unitOfWork.Tasks.Add(task);

        await unitOfWork.CommitAsync();

        return new Client.EditProblem
        {
            Id = task.Id,
            Name = task.Name,
            Statement = task.Statement,
            MemoryLimitBytes = task.MemoryLimitBytes,
            TestsFolder = task.TestsFolder,
            IsOpened = task.IsOpened,
            TimeLimitMilliseconds = task.TimeLimitMilliseconds
        };
    }
}
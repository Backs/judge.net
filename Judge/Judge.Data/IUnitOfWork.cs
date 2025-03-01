using System;
using Judge.Model.Account;
using Judge.Model.CheckSolution;
using Judge.Model.Configuration;
using Judge.Model.Contests;
using Judge.Model.SubmitSolution;
using Task = System.Threading.Tasks.Task;

namespace Judge.Data;

public interface IUnitOfWork : IDisposable, IAsyncDisposable
{
    void Commit();
    Task CommitAsync();
    IContestsRepository Contests { get; }
    IContestTaskRepository ContestTasks { get; }
    ILanguageRepository Languages { get; }
    ISubmitRepository Submits { get; }
    ISubmitResultRepository SubmitResults { get; }
    ITaskRepository Tasks { get; }
    IUserRepository Users { get; }
}
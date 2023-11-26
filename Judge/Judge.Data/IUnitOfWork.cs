using System;
using Judge.Model.Account;
using Judge.Model.CheckSolution;
using Judge.Model.Configuration;
using Judge.Model.Contests;
using Judge.Model.SubmitSolution;
using Task = System.Threading.Tasks.Task;

namespace Judge.Data
{
    public interface IUnitOfWork : IDisposable, IAsyncDisposable
    {
        void Commit();
        Task CommitAsync();
        IContestResultRepository ContestResultRepository { get; }
        IContestsRepository ContestsRepository { get; }
        IContestTaskRepository ContestTaskRepository { get; }
        ILanguageRepository LanguageRepository { get; }
        ISubmitRepository SubmitRepository { get; }
        ISubmitResultRepository SubmitResultRepository { get; }
        ITaskNameRepository TaskNameRepository { get; }
        ITaskRepository TaskRepository { get; }
        IUserRepository UserRepository { get; }
    }
}

using System.Threading.Tasks;
using Judge.Data.Repository;
using Judge.Model.Account;
using Judge.Model.CheckSolution;
using Judge.Model.Configuration;
using Judge.Model.Contests;
using Judge.Model.SubmitSolution;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Task = System.Threading.Tasks.Task;

namespace Judge.Data
{
    internal sealed class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext context;
        private IContestResultRepository? contestResultRepository;
        private IContestsRepository? contestsRepository;
        private IContestTaskRepository? contestTaskRepository;
        private ILanguageRepository? languageRepository;
        private ISubmitRepository? submitRepository;
        private ISubmitResultRepository? submitResultRepository;
        private ITaskNameRepository? taskNameRepository;
        private ITaskRepository? taskRepository;
        private IUserRepository? userRepository;
        private readonly IDbContextTransaction? transaction;

        public UnitOfWork(DataContext context, bool startTransaction)
        {
            this.context = context;
            if (startTransaction)
            {
                this.transaction = this.context.Database.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
            }
        }

        public void Dispose()
        {
            this.transaction?.Dispose();
            this.context.Dispose();
        }

        public void Commit()
        {
            this.transaction?.Commit();
            this.context.SaveChanges();
        }

        public async Task CommitAsync()
        {
            if (this.transaction != null)
                await this.transaction.CommitAsync();
            await this.context.SaveChangesAsync();
        }

        public IContestResultRepository ContestResults =>
            this.contestResultRepository ??= new ContestResultRepository(this.context);

        public IContestsRepository Contests =>
            this.contestsRepository ??= new ContestsRepository(this.context);

        public IContestTaskRepository ContestTasks =>
            this.contestTaskRepository ??= new ContestTaskRepository(this.context);

        public ILanguageRepository Languages =>
            this.languageRepository ??= new LanguageRepository(this.context);

        public ISubmitRepository Submits =>
            this.submitRepository ??= new SubmitRepository(this.context);

        public ISubmitResultRepository SubmitResults =>
            this.submitResultRepository ??= new SubmitResultRepository(this.context);

        public ITaskNameRepository TaskNames =>
            this.taskNameRepository ??= new TaskNameRepository(this.context);

        public ITaskRepository Tasks =>
            this.taskRepository ??= new TaskRepository(this.context);

        public IUserRepository Users =>
            this.userRepository ??= new UserRepository(this.context);

        public async ValueTask DisposeAsync()
        {
            if (this.transaction != null)
                await this.transaction.DisposeAsync();
            await this.context.DisposeAsync();
        }
    }
}
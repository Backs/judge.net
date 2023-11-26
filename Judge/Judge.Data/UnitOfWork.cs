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
        private IContestResultRepository contestResultRepository;
        private IContestsRepository contestsRepository;
        private IContestTaskRepository contestTaskRepository;
        private ILanguageRepository languageRepository;
        private ISubmitRepository submitRepository;
        private ISubmitResultRepository submitResultRepository;
        private ITaskNameRepository taskNameRepository;
        private ITaskRepository taskRepository;
        private IUserRepository userRepository;
        private readonly IDbContextTransaction transaction;

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
            this.context?.Dispose();
        }

        public void Commit()
        {
            this.transaction?.Commit();
            this.context?.SaveChanges();
        }

        public async Task CommitAsync()
        {
            if (this.transaction != null)
                await this.transaction.CommitAsync();
            if (this.context != null)
                await this.context.SaveChangesAsync();
        }

        public IContestResultRepository ContestResultRepository => this.contestResultRepository ??
                                                                   (this.contestResultRepository =
                                                                       new ContestResultRepository(this.context));

        public IContestsRepository ContestsRepository => this.contestsRepository ??
                                                         (this.contestsRepository =
                                                             new ContestsRepository(this.context));

        public IContestTaskRepository ContestTaskRepository => this.contestTaskRepository ??
                                                               (this.contestTaskRepository =
                                                                   new ContestTaskRepository(this.context));

        public ILanguageRepository LanguageRepository => this.languageRepository ??
                                                         (this.languageRepository =
                                                             new LanguageRepository(this.context));

        public ISubmitRepository SubmitRepository =>
            this.submitRepository ?? (this.submitRepository = new SubmitRepository(this.context));

        public ISubmitResultRepository SubmitResultRepository => this.submitResultRepository ??
                                                                 (this.submitResultRepository =
                                                                     new SubmitResultRepository(this.context));

        public ITaskNameRepository TaskNameRepository => this.taskNameRepository ??
                                                         (this.taskNameRepository =
                                                             new TaskNameRepository(this.context));

        public ITaskRepository TaskRepository =>
            this.taskRepository ?? (this.taskRepository = new TaskRepository(this.context));

        public IUserRepository UserRepository =>
            this.userRepository ?? (this.userRepository = new UserRepository(this.context));

        public async ValueTask DisposeAsync()
        {
            if (this.transaction != null)
                await this.transaction.DisposeAsync();
            if (this.context != null)
                await this.context.DisposeAsync();
        }
    }
}
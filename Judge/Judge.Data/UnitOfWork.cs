using Judge.Data.Repository;
using Judge.Model.Account;
using Judge.Model.CheckSolution;
using Judge.Model.Configuration;
using Judge.Model.Contests;
using Judge.Model.SubmitSolution;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Judge.Data
{
    internal sealed class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _context;
        private IContestResultRepository _contestResultRepository;
        private IContestsRepository _contestsRepository;
        private IContestTaskRepository _contestTaskRepository;
        private ILanguageRepository _languageRepository;
        private ISubmitRepository _submitRepository;
        private ISubmitResultRepository _submitResultRepository;
        private ITaskNameRepository _taskNameRepository;
        private ITaskRepository _taskRepository;
        private IUserRepository _userRepository;
        private readonly IDbContextTransaction _transaction;

        public UnitOfWork(DataContext context, bool startTransaction)
        {
            this._context = context;
            if (startTransaction)
            {
                this._transaction = this._context.Database.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
            }
        }
        public void Dispose()
        {
            _transaction?.Dispose();
            _context?.Dispose();
        }

        public void Commit()
        {
            _transaction?.Commit();
            _context?.SaveChanges();
        }

        public IContestResultRepository ContestResultRepository => _contestResultRepository ?? (_contestResultRepository = new ContestResultRepository(_context));

        public IContestsRepository ContestsRepository => _contestsRepository ?? (_contestsRepository = new ContestsRepository(_context));

        public IContestTaskRepository ContestTaskRepository => _contestTaskRepository ?? (_contestTaskRepository = new ContestTaskRepository(_context));

        public ILanguageRepository LanguageRepository => _languageRepository ?? (_languageRepository = new LanguageRepository(_context));

        public ISubmitRepository SubmitRepository => _submitRepository ?? (_submitRepository = new SubmitRepository(_context));

        public ISubmitResultRepository SubmitResultRepository => _submitResultRepository ?? (_submitResultRepository = new SubmitResultRepository(_context));

        public ITaskNameRepository TaskNameRepository => _taskNameRepository ?? (_taskNameRepository = new TaskNameRepository(_context));

        public ITaskRepository TaskRepository => _taskRepository ?? (_taskRepository = new TaskRepository(_context));

        public IUserRepository UserRepository => _userRepository ?? (_userRepository = new UserRepository(_context));
    }
}

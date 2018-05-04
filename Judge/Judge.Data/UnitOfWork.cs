using System;
using System.Transactions;
using SimpleInjector;

namespace Judge.Data
{
    internal sealed class UnitOfWork : IUnitOfWork
    {
        private readonly TransactionScope _transactionScope;
        private readonly Container _container;
        private readonly DataContext _context;

        public UnitOfWork(bool transactionRequired, Container container, DataContext context)
        {
            _container = container;
            _context = context;
            if (transactionRequired)
            {
                _transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted });
            }
        }
        public void Dispose()
        {
            _transactionScope?.Dispose();
        }

        public void Commit()
        {
            if (_transactionScope == null)
            {
                throw new InvalidOperationException("Commit transaction called, but transaction wasn't opened");
            }

            if (_context != null)
            {
                _context.SaveChanges();
                _transactionScope.Complete();
            }
        }

        public T GetRepository<T>() where T : class
        {
            return _container.GetInstance<T>();
        }
    }
}

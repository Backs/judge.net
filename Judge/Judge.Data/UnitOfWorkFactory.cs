using SimpleInjector;

namespace Judge.Data
{
    internal sealed class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        private readonly DataContext _context;
        private readonly Container _container;

        public UnitOfWorkFactory(DataContext context, Container container)
        {
            _context = context;
            _container = container;
        }

        public IUnitOfWork GetUnitOfWork(bool transactionRequired)
        {
            return new UnitOfWork(transactionRequired, _container, _context);
        }
    }
}

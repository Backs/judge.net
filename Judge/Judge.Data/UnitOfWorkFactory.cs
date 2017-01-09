using System;
using Microsoft.Practices.Unity;

namespace Judge.Data
{
    internal sealed class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        private readonly Func<DataContext> _contextCreator;
        private readonly IUnityContainer _container;

        public UnitOfWorkFactory(Func<DataContext> contextCreator, IUnityContainer container)
        {
            _contextCreator = contextCreator;
            _container = container;
        }

        public IUnitOfWork GetUnitOfWork(bool transactionRequired)
        {
            return new UnitOfWork(transactionRequired, _container, _contextCreator);
        }
    }
}

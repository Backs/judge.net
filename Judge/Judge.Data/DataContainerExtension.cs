using Judge.Data.Repository;
using Judge.Model.Entities;
using Judge.Model.SubmitSolution;
using Microsoft.AspNet.Identity;
using Microsoft.Practices.Unity;

namespace Judge.Data
{
    public sealed class DataContainerExtension : UnityContainerExtension
    {
        private readonly string _connectionString;

        public DataContainerExtension(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void Initialize()
        {
            Container.RegisterType<DataContext>(new PerRequestLifetimeManager(), new InjectionConstructor(_connectionString));

            Container.RegisterType<IUnitOfWorkFactory, UnitOfWorkFactory>(new PerRequestLifetimeManager());

            Container.RegisterType<IUserPasswordStore<User, long>, UserStore>(new PerRequestLifetimeManager());
            Container.RegisterType<IUserStore<User, long>, UserStore>(new PerRequestLifetimeManager());

            Container.RegisterType<ISubmitRepository, SubmitRepository>(new PerRequestLifetimeManager());
        }
    }
}

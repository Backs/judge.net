using Judge.Data.Repository;
using Judge.Model.CheckSolution;
using Judge.Model.Configuration;
using Judge.Model.Entities;
using Judge.Model.SubmitSolution;
using Microsoft.AspNet.Identity;
using Microsoft.Practices.Unity;

namespace Judge.Data
{
    public sealed class DataContainerExtension<T> : UnityContainerExtension
        where T : LifetimeManager, new()
    {
        private readonly string _connectionString;

        public DataContainerExtension(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void Initialize()
        {
            Container.RegisterType<DataContext>(new T(), new InjectionConstructor(_connectionString));

            Container.RegisterType<IUnitOfWorkFactory, UnitOfWorkFactory>(new T());

            Container.RegisterType<IUserPasswordStore<User, long>, UserStore>(new T());
            Container.RegisterType<IUserStore<User, long>, UserStore>(new T());

            Container.RegisterType<ISubmitRepository, SubmitRepository>(new T());
            Container.RegisterType<ISubmitResultRepository, SubmitResultRepository>(new T());
            Container.RegisterType<ILanguageRepository, LanguageRepository>(new T());
            Container.RegisterType<ITaskRepository, TaskRepository>(new T());
        }
    }
}

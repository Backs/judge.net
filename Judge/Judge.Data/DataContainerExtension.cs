using Judge.Data.Repository;
using Judge.Model.Account;
using Judge.Model.CheckSolution;
using Judge.Model.Configuration;
using Judge.Model.Contests;
using Judge.Model.Entities;
using Judge.Model.SubmitSolution;
using Microsoft.AspNet.Identity;
using Unity;
using Unity.Extension;
using Unity.Injection;
using Unity.Lifetime;

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
            Container.RegisterType<IUserRepository, UserStore>(new T());

            Container.RegisterType<ISubmitRepository, SubmitRepository>(new T());
            Container.RegisterType<ISubmitResultRepository, SubmitResultRepository>(new T());
            Container.RegisterType<ILanguageRepository, LanguageRepository>(new T());
            Container.RegisterType<ITaskRepository, TaskRepository>(new T());
            Container.RegisterType<ITaskNameRepository, TaskNameRepository>(new T());
            Container.RegisterType<IContestsRepository, ContestsRepository>(new T());
            Container.RegisterType<IContestTaskRepository, ContestTaskRepository>(new T());
            Container.RegisterType<IContestResultRepository, ContestResultRepository>(new T());
        }
    }
}

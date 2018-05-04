using System;
using System.Linq;
using System.Linq.Expressions;
using Judge.Data.Repository;
using Judge.Model.Account;
using Judge.Model.CheckSolution;
using Judge.Model.Configuration;
using Judge.Model.Contests;
using Judge.Model.Entities;
using Judge.Model.SubmitSolution;
using Microsoft.AspNet.Identity;
using SimpleInjector;

namespace Judge.Data
{
    public sealed class DataContainerExtension
    {
        private readonly string _connectionString;
        private readonly Lifestyle _lifestyle;

        public DataContainerExtension(string connectionString, Lifestyle lifestyle)
        {
            _connectionString = connectionString;
            this._lifestyle = lifestyle;
        }

        public void Configure(Container container)
        {
            container.Register<DataContext>(() => new DataContext(_connectionString), _lifestyle);

            container.Register<IUnitOfWorkFactory, UnitOfWorkFactory>(_lifestyle);

            container.Register<IUserPasswordStore<User, long>, UserStore>(_lifestyle);
            container.Register<IUserStore<User, long>, UserStore>(_lifestyle);
            container.Register<IUserRepository, UserStore>(_lifestyle);

            container.Register<ISubmitRepository, SubmitRepository>(_lifestyle);
            container.Register<ISubmitResultRepository, SubmitResultRepository>(_lifestyle);
            container.Register<ILanguageRepository, LanguageRepository>(_lifestyle);
            container.Register<ITaskRepository, TaskRepository>(_lifestyle);
            container.Register<ITaskNameRepository, TaskNameRepository>(_lifestyle);
            container.Register<IContestsRepository, ContestsRepository>(_lifestyle);
            container.Register<IContestTaskRepository, ContestTaskRepository>(_lifestyle);
            container.Register<IContestResultRepository, ContestResultRepository>(_lifestyle);
        }
    }
}

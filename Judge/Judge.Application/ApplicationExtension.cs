using System;
using Judge.Application.Interfaces;
using Judge.Data;
using Judge.Model.Entities;
using Microsoft.AspNet.Identity;
using SimpleInjector;
using SimpleInjector.Integration.Web;

namespace Judge.Application
{
    public sealed class ApplicationExtension
    {
        private readonly string _connectionString;

        public ApplicationExtension(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Configure(Container container)
        {
            new DataContainerExtension(_connectionString, Lifestyle.Scoped).Configure(container);

            container.Register<ISecurityService, SecurityService>(Lifestyle.Scoped);

            container.Register<IProblemsService, ProblemsService>(Lifestyle.Scoped);
            container.Register<ISubmitSolutionService, SubmitSolutionService>(Lifestyle.Scoped);
            container.Register<ISubmitQueueService, SubmitQueueService>(Lifestyle.Scoped);
            container.Register<IContestsService, ContestsService>(Lifestyle.Scoped);
            container.Register<IAdminService, AdminService>(Lifestyle.Scoped);
            container.Register<IUserService, UserService>(Lifestyle.Scoped);
            container.Register<UserManager<User, long>>(Lifestyle.Scoped);
            container.Register<IUserPasswordStore<User, long>, UserStore>(Lifestyle.Scoped);
            container.Register<IUserStore<User, long>, UserStore>(Lifestyle.Scoped);

        }
    }
}

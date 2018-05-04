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
            new DataContainerExtension(_connectionString, new WebRequestLifestyle()).Configure(container);

            container.Register<ISecurityService, SecurityService>(new WebRequestLifestyle());

            container.Register<IProblemsService, ProblemsService>(new WebRequestLifestyle());
            container.Register<ISubmitSolutionService, SubmitSolutionService>(new WebRequestLifestyle());
            container.Register<ISubmitQueueService, SubmitQueueService>(new WebRequestLifestyle());
            container.Register<IContestsService, ContestsService>(new WebRequestLifestyle());
            container.Register<IAdminService, AdminService>(new WebRequestLifestyle());
            container.Register<IUserService, UserService>(new WebRequestLifestyle());
            container.Register<UserManager<User, Int64>>(new WebRequestLifestyle());
        }
    }
}

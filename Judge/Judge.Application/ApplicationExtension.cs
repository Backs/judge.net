namespace Judge.Application
{
    using Judge.Application.Interfaces;
    using Judge.Data;
    using Microsoft.AspNet.Identity;
    using SimpleInjector;

    public sealed class ApplicationExtension
    {
        private readonly string connectionString;

        public ApplicationExtension(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public void Configure(Container container)
        {
            new DataContainerExtension(this.connectionString, Lifestyle.Scoped).Configure(container);

            container.Register<ISecurityService, SecurityService>(Lifestyle.Scoped);

            container.Register<IProblemsService, ProblemsService>(Lifestyle.Scoped);
            container.Register<ISubmitSolutionService, SubmitSolutionService>(Lifestyle.Scoped);
            container.Register<ISubmitQueueService, SubmitQueueService>(Lifestyle.Scoped);
            container.Register<IContestsService, ContestsService>(Lifestyle.Scoped);
            container.Register<IAdminService, AdminService>(Lifestyle.Scoped);
            container.Register<IUserService, UserService>(Lifestyle.Scoped);
            container.Register<UserManager<ApplicationUser, long>>(Lifestyle.Scoped);
            container.Register<IUserPasswordStore<ApplicationUser, long>, UserStore>(Lifestyle.Scoped);
            container.Register<IUserStore<ApplicationUser, long>, UserStore>(Lifestyle.Scoped);

        }
    }
}

using System.Configuration;
using System.Reflection;
using System.Security.Principal;
using System.Web.Mvc;
using Judge.Application;
using Owin;
using SimpleInjector;
using SimpleInjector.Integration.Web;
using SimpleInjector.Integration.Web.Mvc;
using SimpleInjector.Lifestyles;

namespace Judge.Web
{
    public partial class Startup
    {
        public void ConfigureContainer(IAppBuilder app)
        {
            var container = new Container();

            app.Use(async (context, next) =>
            {
                using (AsyncScopedLifestyle.BeginScope(container))
                {
                    CallContextOwinContextAccessor.OwinContext = context;
                    await next();
                }
            });

            container.Options.DefaultScopedLifestyle = new WebRequestLifestyle();

            container.RegisterMvcControllers(Assembly.GetExecutingAssembly());

            container.RegisterInstance<IOwinContextAccessor>(new CallContextOwinContextAccessor());

            container.RegisterInstance<IPrincipal>(new HttpContextPrinciple());

            container.Register<Services.ISessionService, Services.SessionService>(new WebRequestLifestyle());
            var connectionString = ConfigurationManager.ConnectionStrings["DataBaseConnection"].ConnectionString;
            new ApplicationExtension(connectionString).Configure(container);
            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));

            container.Verify();
        }
    }
}
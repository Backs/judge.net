using System.Configuration;
using System.Reflection;
using System.Security.Principal;
using System.Web.Mvc;
using Judge.Application;
using Owin;
using SimpleInjector;
using SimpleInjector.Integration.Web;
using SimpleInjector.Integration.Web.Mvc;

namespace Judge.Web
{
    public partial class Startup
    {
        public void ConfigureContainer(IAppBuilder app)
        {
            var container = new Container();
            container.Options.DefaultScopedLifestyle = new WebRequestLifestyle();

            app.Use(async (context, next) =>
            {
                //using (AsyncScopedLifestyle.BeginScope(container))
                {
                    CallContextOwinContextAccessor.OwinContext = context;
                    await next();
                }
            });

            container.RegisterMvcControllers(Assembly.GetExecutingAssembly());

            container.RegisterInstance<IOwinContextAccessor>(new CallContextOwinContextAccessor());

            container.RegisterInstance<IPrincipal>(new HttpContextPrinciple());

            container.Register<Services.ISessionService, Services.SessionService>(Lifestyle.Scoped);
            var connectionString = ConfigurationManager.ConnectionStrings["DataBaseConnection"].ConnectionString;
            new ApplicationExtension(connectionString).Configure(container);
            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));

            container.Verify();
        }
    }
}
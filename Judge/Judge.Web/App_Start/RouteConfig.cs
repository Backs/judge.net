using System.Web.Mvc;
using System.Web.Routing;

namespace Judge.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "SolutionSubmit",
                url: "Problems/Solution/{submitId}",
                defaults: new { controller = "Problems", Action = "Solution", page = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "ContestTask",
                url: "Contests/{contestId}/Task/{label}",
                defaults: new { controller = "Contests", Action = "Task", page = UrlParameter.Optional }
                );

            routes.MapRoute(
                name: "ProblemsList",
                url: "Problems/Page/{page}",
                defaults: new { controller = "Problems", action = "Index", page = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}

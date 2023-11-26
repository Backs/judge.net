using Judge.Data;
using Microsoft.Extensions.DependencyInjection;

namespace Judge.Services
{
    public static class ServicesExtensions
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddData("Data Source=.;Initial Catalog=Judge;Integrated Security=True");
            services.AddSingleton<IProblemsService, ProblemsService>();
        }
    }
}
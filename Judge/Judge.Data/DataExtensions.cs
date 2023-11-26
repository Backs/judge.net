using Microsoft.Extensions.DependencyInjection;

namespace Judge.Data
{
    public static class DataExtensions
    {
        public static void AddData(this IServiceCollection services, string connectionString)
        {
            services.AddSingleton<IUnitOfWorkFactory>(f => new UnitOfWorkFactory(connectionString));
        }
    }
}
using Judge.Data;
using Judge.Model.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Judge.Services;

public static class ServicesExtensions
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddData("Data Source=.;Initial Catalog=Judge;Integrated Security=True");
        services.AddSingleton<IProblemsService, ProblemsService>();
        services.AddSingleton<ISecurityService, SecurityService>();
        services.AddSingleton<IUsersService, UsersService>();
        services.AddSingleton<IPasswordHasher<User>, PasswordHasher<User>>();
    }
}
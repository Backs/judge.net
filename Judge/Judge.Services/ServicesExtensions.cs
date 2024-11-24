using Judge.Data;
using Judge.Model.Entities;
using Judge.Services.Converters;
using Judge.Services.Converters.Contests;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Judge.Services;

public static class ServicesExtensions
{
    public static void AddServices(this IServiceCollection services, string databaseConnectionString)
    {
        services.AddData(databaseConnectionString);
        services.AddSingleton<IProblemsService, ProblemsService>();
        services.AddSingleton<ISecurityService, SecurityService>();
        services.AddSingleton<IUsersService, UsersService>();
        services.AddSingleton<IPasswordHasher<User>, PasswordHasher<User>>();
        services.AddSingleton<ISubmitsService, SubmitsService>();
        services.AddSingleton<IContestsService, ContestsService>();
        services.AddSingleton<IContestConverterFactory, ContestConverterFactory>();
        services.AddSingleton<ISubmitsConverter, SubmitsConverter>();
        services.AddSingleton<ILanguageService, LanguageService>();
    }
}
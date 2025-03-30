using System;
using System.Configuration;
using System.IO;
using System.Threading;
using Judge.Data;
using Judge.JudgeService.CustomCheckers;
using Judge.JudgeService.Settings;
using Newtonsoft.Json;
using NLog;
using SimpleInjector;
using SimpleInjector.Lifestyles;

namespace Judge.JudgeService;

public static class Program
{
    public static void Main()
    {
        ILogger logger = LogManager.GetLogger("Judge");

        var container = new Container();
        container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

        var connectionString = ConfigurationManager.ConnectionStrings["DataBaseConnection"].ConnectionString;
        var customProblemSettingsPath = ConfigurationManager.AppSettings["CustomProblemSettingsPath"];

        var settings = CustomProblemSettings.Empty;
        if (File.Exists(customProblemSettingsPath))
        {
            settings = GetCustomProblemSettings(customProblemSettingsPath);
        }
        else
        {
            logger.Warn("No custom problem settings found");
        }

        container.RegisterInstance(settings);

        new DataContainerExtension(connectionString, Lifestyle.Scoped).Configure(container);

        container.Register<IJudgeService, JudgeServiceImplementation>(Lifestyle.Scoped);
        container.Register<CheckService>(Lifestyle.Scoped);
        container.Collection.Register<ICustomChecker>(
            [
                typeof(LanguageChecker),
                typeof(ForbiddenCharsChecker),
                typeof(ForbiddenStringsChecker),
                typeof(MaxLengthChecker),
                typeof(PullRequestChecker),
                typeof(CurrentTimeChecker),
                typeof(SolvedChecker)
            ],
            Lifestyle.Scoped);
        container.RegisterSingleton<IProblemSettingsProvider, ProblemSettingsProvider>();
        container.RegisterSingleton<ICustomCheckerService, CustomCheckerService>();
        container.RegisterInstance(logger);

        logger.Info("Service started");
        Console.WriteLine("Press any key to exit...");

        while (true)
        {
            try
            {
                using (AsyncScopedLifestyle.BeginScope(container))
                {
                    var service = container.GetInstance<CheckService>();
                    service.Check();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex);
                Console.ResetColor();
            }

            if (Console.KeyAvailable)
                break;

            Thread.Sleep(TimeSpan.FromSeconds(1));
        }

        logger.Info("Service stopped");
    }

    private static CustomProblemSettings GetCustomProblemSettings(string customProblemSettingsPath)
    {
        var serializer = JsonSerializer.Create(new JsonSerializerSettings());

        using (var sr = new StreamReader(customProblemSettingsPath))
        using (var jsonTextReader = new JsonTextReader(sr))
        {
            return serializer.Deserialize<CustomProblemSettings>(jsonTextReader);
        }
    }
}
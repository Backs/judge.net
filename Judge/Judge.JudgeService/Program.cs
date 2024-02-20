using System;
using System.Configuration;
using System.IO;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;
using Judge.Data;
using Judge.JudgeService.Settings;
using NLog;
using SimpleInjector;
using SimpleInjector.Lifestyles;

namespace Judge.JudgeService
{
    class Program
    {
        static void Main(string[] args)
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
            var serializer = new XmlSerializer(typeof(CustomProblemSettings));

            using (var sr = new StreamReader(customProblemSettingsPath))
            using (var xmlReader = new XmlTextReader(sr))
            {
                return (CustomProblemSettings)serializer.Deserialize(xmlReader);
            }
        }
    }
}
using System;
using System.Configuration;
using System.Threading;
using Judge.Data;
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
    }
}

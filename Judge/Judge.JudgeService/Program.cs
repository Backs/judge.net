using System;
using System.Configuration;
using System.Threading;
using Judge.Data;
using SimpleInjector;
using SimpleInjector.Lifestyles;

namespace Judge.JudgeService
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = new Container();
            container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

            var connectionString = ConfigurationManager.ConnectionStrings["DataBaseConnection"].ConnectionString;
            new DataContainerExtension(connectionString, Lifestyle.Scoped).Configure(container);

            container.Register<IJudgeService, JudgeServiceImplementation>(Lifestyle.Scoped);
            container.Register<CheckService>(Lifestyle.Scoped);

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
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(ex);
                    Console.ResetColor();
                }
                if (Console.KeyAvailable)
                    break;

                Thread.Sleep(TimeSpan.FromSeconds(1));
            }
        }
    }
}

using System;
using System.Configuration;
using System.Threading;
using Judge.Data;
using Unity;
using Unity.Lifetime;

namespace Judge.JudgeService
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = new UnityContainer();
            var connectionString = ConfigurationManager.ConnectionStrings["DataBaseConnection"].ConnectionString;
            container.AddExtension(new DataContainerExtension<HierarchicalLifetimeManager>(connectionString));

            container.RegisterType<IJudgeService, JudgeServiceImplementation>(new HierarchicalLifetimeManager());

            var service = container.Resolve<CheckService>();

            Console.WriteLine("Press any key to exit...");

            while (true)
            {
                try
                {
                    service.Check();
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

using System.Configuration;
using Judge.Data;
using Microsoft.Practices.Unity;

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

            service.Check();
        }
    }
}

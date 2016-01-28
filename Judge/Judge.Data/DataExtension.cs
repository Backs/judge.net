using Judge.Model.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.Practices.Unity;

namespace Judge.Data
{
    public sealed class DataExtension : UnityContainerExtension
    {
        private readonly string _connectionString;

        public DataExtension(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void Initialize()
        {
            Container.RegisterType<IUserPasswordStore<User, long>, UserStore>(new PerRequestLifetimeManager(), new InjectionConstructor(_connectionString));
            Container.RegisterType<IUserStore<User, long>, UserStore>(new PerRequestLifetimeManager(), new InjectionConstructor(_connectionString));
        }
    }
}

using SimpleInjector;

namespace Judge.Data
{
    public sealed class DataContainerExtension
    {
        private readonly string _connectionString;
        private readonly Lifestyle _lifestyle;

        public DataContainerExtension(string connectionString, Lifestyle lifestyle)
        {
            _connectionString = connectionString;
            this._lifestyle = lifestyle;
        }

        public void Configure(Container container)
        {
            container.Register<IUnitOfWorkFactory>(() => new UnitOfWorkFactory(_connectionString), _lifestyle);
        }
    }
}

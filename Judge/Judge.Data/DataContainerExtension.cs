using SimpleInjector;

namespace Judge.Data
{
    public sealed class DataContainerExtension
    {
        private readonly string connectionString;
        private readonly Lifestyle lifestyle;

        public DataContainerExtension(string connectionString, Lifestyle lifestyle)
        {
            this.connectionString = connectionString;
            this.lifestyle = lifestyle;
        }

        public void Configure(Container container)
        {
            container.Register<IUnitOfWorkFactory>(() => new UnitOfWorkFactory(this.connectionString), this.lifestyle);
        }
    }
}

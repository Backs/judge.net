namespace Judge.Data
{
    internal sealed class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        private readonly string connectionString;

        public UnitOfWorkFactory(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public IUnitOfWork GetUnitOfWork()
        {
            return this.GetUnitOfWork(false);
        }

        public IUnitOfWork GetUnitOfWork(bool startTransaction)
        {
            return new UnitOfWork(new DataContext(this.connectionString), startTransaction);
        }
    }
}

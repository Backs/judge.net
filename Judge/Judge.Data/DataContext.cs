using System.Data.Entity;

namespace Judge.Data
{
    internal sealed class DataContext : DbContext
    {
        public DataContext(string connectionString)
            : base(connectionString)
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Configurations.Add(new LanguageMapping());
        }
    }
}

using System.Data.Entity;
using Judge.Data.Mappings;

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
            modelBuilder.Configurations.Add(new UserMapping());
            modelBuilder.Configurations.Add(new SubmitMapping());
            modelBuilder.Configurations.Add(new CheckQueueMapping());
            modelBuilder.Configurations.Add(new SubmitResultMapping());
        }
    }
}

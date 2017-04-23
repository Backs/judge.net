using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using Judge.Data.Mappings;
using Judge.Model.SubmitSolution;

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
            modelBuilder.Configurations.Add(new TaskMapping());
            modelBuilder.Configurations.Add(new ContestMapping());
        }

        public CheckQueue DequeueSubmitCheck()
        {
            return ObjectContext.ExecuteStoreQuery<CheckQueue>("EXEC dbo.DequeueSubmitCheck").FirstOrDefault();
        }

        private ObjectContext ObjectContext => ((IObjectContextAdapter)this).ObjectContext;
    }
}

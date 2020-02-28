using System.Linq;
using Judge.Data.Mappings;
using Judge.Model.SubmitSolution;
using Microsoft.EntityFrameworkCore;

namespace Judge.Data
{
    internal sealed class DataContext : DbContext
    {
        private readonly string _connectionString;

        public DataContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new LanguageMapping());
            modelBuilder.ApplyConfiguration(new UserMapping());
            modelBuilder.ApplyConfiguration(new SubmitBaseMapping());
            modelBuilder.ApplyConfiguration(new CheckQueueMapping());
            modelBuilder.ApplyConfiguration(new SubmitResultMapping());
            modelBuilder.ApplyConfiguration(new TaskMapping());
            modelBuilder.ApplyConfiguration(new ContestMapping());
            modelBuilder.ApplyConfiguration(new ContestTaskMapping());
            modelBuilder.ApplyConfiguration(new UserRoleMapping());
        }

        public CheckQueue DequeueSubmitCheck()
        {
            return this.Set<CheckQueue>().FromSqlRaw("EXEC dbo.DequeueSubmitCheck").AsEnumerable().FirstOrDefault();
        }
    }
}

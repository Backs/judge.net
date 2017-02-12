using System.ComponentModel.DataAnnotations.Schema;
using Judge.Model.CheckSolution;
using System.Data.Entity.ModelConfiguration;

namespace Judge.Data.Mappings
{
    internal sealed class TaskMapping : EntityTypeConfiguration<Task>
    {
        public TaskMapping()
        {
            HasKey(o => o.Id);
            Property(o => o.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(o => o.CreationDateUtc).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            ToTable("Tasks");
        }
    }
}

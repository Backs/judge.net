using Judge.Model.CheckSolution;
using System.Data.Entity.ModelConfiguration;

namespace Judge.Data.Mappings
{
    internal sealed class TaskMapping : EntityTypeConfiguration<Task>
    {
        public TaskMapping()
        {
            HasKey(o => o.Id);

            ToTable("Tasks");
        }
    }
}

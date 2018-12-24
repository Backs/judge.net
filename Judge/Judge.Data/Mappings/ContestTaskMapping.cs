using Judge.Model.Contests;
using System.Data.Entity.ModelConfiguration;

namespace Judge.Data.Mappings
{
    internal sealed class ContestTaskMapping : EntityTypeConfiguration<ContestTask>
    {
        public ContestTaskMapping()
        {
            HasKey(o => new { o.TaskName });

            HasRequired(o => o.Task)
                .WithMany()
                .HasForeignKey(o => o.TaskId);

            HasRequired(o => o.Contest)
                .WithMany()
                .HasForeignKey(o => o.ContestId);

            ToTable("ContestTasks");
        }
    }
}
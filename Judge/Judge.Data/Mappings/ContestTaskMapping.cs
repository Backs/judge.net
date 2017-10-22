using System.Data.Entity.ModelConfiguration;
using Judge.Model.Contests;

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
                .Map(map => map.MapKey("ContestId"));

            ToTable("ContestTasks");
        }
    }
}
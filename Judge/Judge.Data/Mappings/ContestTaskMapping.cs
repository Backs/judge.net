using System.Data.Entity.ModelConfiguration;
using Judge.Model.Contests;

namespace Judge.Data.Mappings
{
    internal sealed class ContestTaskMapping : EntityTypeConfiguration<ContestTask>
    {
        public ContestTaskMapping()
        {
            HasKey(o => new { o.TaskName, o.ContestId });

            HasRequired(o => o.Task)
                .WithMany()
                .Map(map => map.MapKey("TaskId"));

            ToTable("ContestTasks");
        }
    }
}
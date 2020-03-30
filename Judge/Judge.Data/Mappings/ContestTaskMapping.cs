using Judge.Model.Contests;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Judge.Data.Mappings
{
    internal sealed class ContestTaskMapping : IEntityTypeConfiguration<ContestTask>
    {
        public void Configure(EntityTypeBuilder<ContestTask> builder)
        {
            builder.HasKey(o => new { o.ContestId, o.TaskName });

            builder.HasOne(o => o.Task)
                .WithMany()
                .HasForeignKey(o => o.TaskId);

            builder.HasOne(o => o.Contest)
                .WithMany()
                .HasForeignKey(o => o.ContestId);

            builder.ToTable("ContestTasks");
        }
    }
}
using Judge.Model.CheckSolution;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Judge.Data.Mappings
{
    internal sealed class TaskMapping : IEntityTypeConfiguration<Task>
    {
        public void Configure(EntityTypeBuilder<Task> builder)
        {
            builder.HasKey(o => o.Id);
            builder.Property(o => o.Id).UseIdentityColumn();
            builder.Property(o => o.CreationDateUtc).ValueGeneratedOnAdd();

            builder.ToTable("Tasks");
        }
    }
}

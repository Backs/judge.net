using Judge.Model.SubmitSolution;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Judge.Data.Mappings
{
    internal sealed class SubmitBaseMapping : IEntityTypeConfiguration<SubmitBase>
    {
        public void Configure(EntityTypeBuilder<SubmitBase> builder)
        {
            builder.HasKey(o => o.Id);
            builder.Property(o => o.Id).UseIdentityColumn();
            builder.Property(o => o.SubmitDateUtc).ValueGeneratedOnAdd();
            builder.Property<byte>("SubmitType");

            builder.HasDiscriminator<byte>(@"SubmitType").HasValue<ProblemSubmit>(1)
                .HasValue<ContestTaskSubmit>(2);

            builder.HasMany(o => o.Results)
                .WithOne(o => o.Submit)
                .HasForeignKey("SubmitId");

            builder.ToTable("Submits", "dbo");
        }
    }
}

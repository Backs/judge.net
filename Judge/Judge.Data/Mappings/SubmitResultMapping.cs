using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Judge.Model.SubmitSolution;

namespace Judge.Data.Mappings
{
    internal sealed class SubmitResultMapping : EntityTypeConfiguration<SubmitResult>
    {
        public SubmitResultMapping()
        {
            HasKey(o => o.Id);
            Property(o => o.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            HasOptional(o => o.CheckQueue)
                .WithOptionalPrincipal()
                .Map(map => map.MapKey("SubmitResultId"));

            ToTable("SubmitResults", "dbo");
        }
    }
}

using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Judge.Model.SubmitSolution;

namespace Judge.Data.Mappings
{
    internal sealed class SubmitMapping : EntityTypeConfiguration<Submit>
    {
        public SubmitMapping()
        {
            HasKey(o => o.Id);
            Property(o => o.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(o => o.SubmitDateUtc).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            HasMany(o => o.Results)
                .WithRequired(o => o.Submit)
                .Map(map => map.MapKey("SubmitId"));

            ToTable("Submits", "dbo");
        }
    }
}

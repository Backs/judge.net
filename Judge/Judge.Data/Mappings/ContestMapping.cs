using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Judge.Model.Contests;

namespace Judge.Data.Mappings
{
    internal sealed class ContestMapping : EntityTypeConfiguration<Contest>
    {
        public ContestMapping()
        {
            HasKey(o => o.Id);
            Property(o => o.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            ToTable("Contests");
        }
    }
}

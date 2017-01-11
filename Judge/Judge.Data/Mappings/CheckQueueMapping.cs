using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Judge.Model.SubmitSolution;

namespace Judge.Data.Mappings
{
    internal sealed class CheckQueueMapping : EntityTypeConfiguration<CheckQueue>
    {
        public CheckQueueMapping()
        {
            HasKey(o => o.Id);
            Property(o => o.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(o => o.CreationDateUtc).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            ToTable("CheckQueue", "dbo");
        }
    }
}

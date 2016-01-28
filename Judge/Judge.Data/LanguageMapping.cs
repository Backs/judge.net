using System.Data.Entity.ModelConfiguration;
using Judge.Model.Entities;

namespace Judge.Data
{
    internal sealed class LanguageMapping : EntityTypeConfiguration<Language>
    {
        public LanguageMapping()
        {
            HasKey(o => o.Id);

            ToTable("Languages");
        }
    }
}

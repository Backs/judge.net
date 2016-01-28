using System.Data.Entity.ModelConfiguration;
using Judge.Model.Entities;

namespace Judge.Data.Mappings
{
    internal sealed class UserMapping : EntityTypeConfiguration<User>
    {
        public UserMapping()
        {
            HasKey(o => o.Id);
            ToTable("Users");
        }
    }
}

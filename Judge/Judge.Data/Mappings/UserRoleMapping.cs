using System.Data.Entity.ModelConfiguration;
using Judge.Model.Entities;

namespace Judge.Data.Mappings
{
    internal sealed class UserRoleMapping : EntityTypeConfiguration<UserRole>
    {
        public UserRoleMapping()
        {
            HasKey(o => o.Id);

            ToTable("UserRoles");
        }
    }
}

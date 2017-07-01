using System.Data.Entity.ModelConfiguration;
using Judge.Model.Entities;

namespace Judge.Data.Mappings
{
    internal sealed class UserRoleMapping : EntityTypeConfiguration<UserRole>
    {
        public UserRoleMapping()
        {
            HasKey(o => o.Id);
            HasRequired(o => o.User)
                .WithMany()
                .Map(map => map.MapKey("UserId"));

            ToTable("UserRoles");
        }
    }
}

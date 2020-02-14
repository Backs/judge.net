using System.Data.Entity.ModelConfiguration;
using Judge.Model.Entities;

namespace Judge.Data.Mappings
{
    internal sealed class UserMapping : EntityTypeConfiguration<User>
    {
        public UserMapping()
        {
            HasKey(o => o.Id);
            HasMany(o => o.UserRoles)
                .WithRequired()
                .HasForeignKey(o => o.UserId);

            ToTable("Users");
        }
    }
}

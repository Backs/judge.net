using Judge.Model.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Judge.Data.Mappings
{
    internal sealed class UserMapping : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(o => o.Id);
            builder.HasMany(o => o.UserRoles)
                .WithOne()
                .HasForeignKey(o => o.UserId);

            builder.ToTable("Users");
        }
    }
}

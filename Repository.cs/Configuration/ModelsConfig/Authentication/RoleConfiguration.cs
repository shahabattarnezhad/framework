using Entities.Models.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.Configuration.ModelsConfig.Authentication;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasMany(entity => entity.UserRoles)
               .WithOne()
               .HasForeignKey(x => x.RoleId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(entity => entity.NoticeRoles)
               .WithOne(x => x.Role)
               .HasForeignKey(x => x.RoleId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}

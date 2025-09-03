using Entities.Models.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.Configuration.ModelsConfig.Authentication;

public class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
{
    public void Configure(EntityTypeBuilder<RolePermission> builder)
    {
        builder.HasKey(entity => new { entity.RoleId, entity.PermissionId });

        builder.HasOne(entity => entity.Role)
               .WithMany(x => x.RolePermissions)
               .HasForeignKey(entity => entity.RoleId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(entity => entity.Permission)
               .WithMany(x => x.RolePermissions)
               .HasForeignKey(entity => entity.PermissionId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}

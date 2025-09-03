using Entities.Models.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.Configuration.ModelsConfig.Authentication;

public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.HasKey(entity => entity.Id);

        builder.Property(entity => entity.Description)
               .HasMaxLength(1000);

        builder.Property(entity => entity.Name)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(entity => entity.DisplayName)
               .HasMaxLength(100);

        builder.HasMany(entity => entity.RolePermissions)
               .WithOne(x => x.Permission)
               .HasForeignKey(x => x.PermissionId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}

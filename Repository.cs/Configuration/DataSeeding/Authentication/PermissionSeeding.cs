using Entities.Models.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Repository.Configuration.DataSeeding.Constants;

namespace Repository.Configuration.DataSeeding.Authentication;

public class PermissionSeeding : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.HasData(
            new Permission()
            {
                Id = SeedConstants.UsersReadId,
                Name = SeedConstants.UsersReadName,
                DisplayName = SeedConstants.UsersReadDisplayName,
            },
            new Permission()
            {
                Id = SeedConstants.UsersCreateId,
                Name = SeedConstants.UsersCreateName,
                DisplayName = SeedConstants.UsersCreateDisplayName,
            },
            new Permission()
            {
                Id = SeedConstants.UsersUpdateId,
                Name = SeedConstants.UsersUpdateName,
                DisplayName = SeedConstants.UsersUpdateDisplayName,
            },
            new Permission()
            {
                Id = SeedConstants.UsersDeleteId,
                Name = SeedConstants.UsersDeleteName,
                DisplayName = SeedConstants.UsersDeleteDisplayName,
            }

        );
    }
}

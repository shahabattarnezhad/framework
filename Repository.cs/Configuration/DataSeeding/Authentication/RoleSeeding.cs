using Entities.Models.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Repository.Configuration.DataSeeding.Constants;

namespace Repository.Configuration.DataSeeding.Authentication;

public class RoleSeeding : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasData(
            new Role()
            {
                Id = SeedConstants.AdminRoleId,
                Name = "Admin",
                NormalizedName = "ADMIN"
            },
            new Role()
            {
                Id = SeedConstants.ClientRoleId,
                Name = "Client",
                NormalizedName = "CLIENT"
            },
            new Role()
            {
                Id = SeedConstants.AccountantRoleId,
                Name = "Accountant",
                NormalizedName = "ACCOUNTANT"
            },
            new Role()
            {
                Id = SeedConstants.ManagerRoleId,
                Name = "Manager",
                NormalizedName = "MANAGER"
            },
            new Role()
            {
                Id = SeedConstants.DriverRoleId,
                Name = "Driver",
                NormalizedName = "DRIVER"
            }
        );
    }
}

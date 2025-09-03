using Entities.Models.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Repository.Configuration.DataSeeding.Constants;

namespace Repository.Configuration.DataSeeding.Authentication;

public class UserRoleSeeding : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.HasData(
            new UserRole()
            {
                UserId = SeedConstants.AdminUserId,
                RoleId = SeedConstants.AdminRoleId,
            }
        );
    }
}

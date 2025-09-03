using Entities.Models.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Repository.Configuration.DataSeeding.Constants;

namespace Repository.Configuration.DataSeeding.Authentication;

public class UserSeeding : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasData(
            new User()
            {
                Id = SeedConstants.AdminUserId,
                Email = "atarnezhad@gmail.com",
                FirstName = "Shahab",
                LastName = "Attarnejad",
                UserName = "atarnezhad",
                NormalizedUserName = "ATARNEZHAD",
                NormalizedEmail = "ATARNEZHAD@GMAIL.COM",
                LockoutEnabled = true,
                EmailConfirmed = true,
                SecurityStamp = string.Empty,
                PasswordHash = new PasswordHasher<User>().HashPassword(null!, "Comet@123"),
            }
        );
    }
}

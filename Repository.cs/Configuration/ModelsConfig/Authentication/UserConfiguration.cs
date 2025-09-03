using Entities.Models.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.Configuration.ModelsConfig.Authentication;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(entity => entity.FirstName)
               .HasMaxLength(50);

        builder.Property(entity => entity.LastName)
               .HasMaxLength(50);

        builder.Property(entity => entity.NationalCode)
               .IsRequired()
               .HasMaxLength(10);

        builder.HasOne(entity => entity.ProfileImage)
               .WithOne(x => x.User)
               .HasForeignKey<UserProfileImage>(entity => entity.UserId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(entity => entity.UserProfile)
               .WithOne(x => x.User)
               .HasForeignKey<UserProfile>(entity => entity.UserId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(entity => entity.UserRoles)
               .WithOne()
               .HasForeignKey(x => x.UserId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}

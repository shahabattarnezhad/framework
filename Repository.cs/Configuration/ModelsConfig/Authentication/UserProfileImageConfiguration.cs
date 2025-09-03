using Entities.Models.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.Configuration.ModelsConfig.Authentication;

public class UserProfileImageConfiguration : IEntityTypeConfiguration<UserProfileImage>
{
    public void Configure(EntityTypeBuilder<UserProfileImage> builder)
    {
        builder.HasKey(entity => entity.Id);

        builder.Property(entity => entity.ProfileImageSmallUrl)
               .HasMaxLength(500);

        builder.Property(entity => entity.ProfileImageMediumUrl)
               .HasMaxLength(500);

        builder.Property(entity => entity.ProfileImageLargeUrl)
               .HasMaxLength(500);

        builder.Property(entity => entity.ProfileImageOriginalUrl)
               .HasMaxLength(500);

        builder.Property(entity => entity.Type)
               .HasMaxLength(100);

        builder.Property(entity => entity.AltText)
               .HasMaxLength(150);

        builder.HasOne(entity => entity.User)
               .WithOne(x => x.ProfileImage)
               .HasForeignKey<UserProfileImage>(entity => entity.UserId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}

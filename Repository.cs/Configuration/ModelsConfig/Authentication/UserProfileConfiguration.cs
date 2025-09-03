using Entities.Models.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.Configuration.ModelsConfig.Authentication;

public class UserProfileConfiguration : IEntityTypeConfiguration<UserProfile>
{
    public void Configure(EntityTypeBuilder<UserProfile> builder)
    {
        builder.HasKey(entity => entity.Id);

        builder.Property(entity => entity.Description)
               .HasMaxLength(1000);

        builder.Property(entity => entity.Bio)
               .HasMaxLength(300);

        builder.Property(entity => entity.FatherName)
               .HasMaxLength(50);

        builder.Property(entity => entity.ProfilePictureUrl)
               .HasMaxLength(150);

        builder.HasOne(entity => entity.User)
               .WithOne(x => x.UserProfile)
               .HasForeignKey <UserProfile>(entity => entity.UserId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}

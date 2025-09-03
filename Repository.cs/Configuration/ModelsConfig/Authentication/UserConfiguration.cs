using Entities.Models.Authentication;
using Entities.Models.Users;
using Entities.Models.Users.Base;
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

        builder.HasOne(entity => entity.DriverProfile)
               .WithOne(x => x.User)
               .HasForeignKey<Driver>(entity => entity.UserId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(entity => entity.ParentProfile)
               .WithOne(x => x.User)
               .HasForeignKey<Parent>(entity => entity.UserId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(entity => entity.StudentProfile)
               .WithOne(x => x.User)
               .HasForeignKey<Student>(entity => entity.UserId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(entity => entity.TeacherProfile)
               .WithOne(x => x.User)
               .HasForeignKey<Teacher>(entity => entity.UserId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(entity => entity.UserProfile)
               .WithOne(x => x.User)
               .HasForeignKey<UserProfile>(entity => entity.UserId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(entity => entity.AdminProfile)
               .WithOne(x => x.User)
               .HasForeignKey<Administrator>(entity => entity.UserId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(entity => entity.AccountantProfile)
               .WithOne(x => x.User)
               .HasForeignKey<Accountant>(entity => entity.UserId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(entity => entity.ManagerProfile)
               .WithOne(x => x.User)
               .HasForeignKey<Manager>(entity => entity.UserId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(entity => entity.VicePrincipalProfile)
               .WithOne(x => x.User)
               .HasForeignKey<VicePrincipal>(entity => entity.UserId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(entity => entity.Department)
               .WithMany(x => x.Users)
               .HasForeignKey(entity => entity.DepartmentId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(entity => entity.Position)
               .WithMany(x => x.Users)
               .HasForeignKey(entity => entity.PositionId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(entity => entity.School)
               .WithMany(x => x.Users)
               .HasForeignKey(entity => entity.SchoolId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(entity => entity.Notices)
               .WithOne(x => x.Publisher)
               .HasForeignKey(x => x.PublisherId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(entity => entity.Contacts)
               .WithOne(x => x.User)
               .HasForeignKey(x => x.UserId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(entity => entity.SentMessages)
               .WithOne(x => x.SenderUser)
               .HasForeignKey(x => x.SenderUserId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(entity => entity.ReceivedMessages)
               .WithOne(x => x.ReceiverUser)
               .HasForeignKey(x => x.ReceiverUserId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(entity => entity.Addresses)
               .WithOne(x => x.User)
               .HasForeignKey(x => x.UserId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(entity => entity.Feedbacks)
               .WithOne(x => x.Staff)
               .HasForeignKey(x => x.StaffId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(entity => entity.UserRoles)
               .WithOne()
               .HasForeignKey(x => x.UserId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(entity => entity.Documents)
               .WithOne(x => x.User)
               .HasForeignKey(x => x.UserId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(entity => entity.AuditTrails)
               .WithOne(x => x.User)
               .HasForeignKey(x => x.PerformedById)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(entity => entity.ActivityLogs)
               .WithOne(x => x.User)
               .HasForeignKey(x => x.UserId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(entity => entity.Notifications)
               .WithOne(x => x.User)
               .HasForeignKey(x => x.UserId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(entity => entity.FileResources)
               .WithOne(x => x.User)
               .HasForeignKey(x => x.UserId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(entity => entity.LeaveRequests)
               .WithOne(x => x.User)
               .HasForeignKey(x => x.UserId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(entity => entity.SystemBackups)
               .WithOne(x => x.User)
               .HasForeignKey(x => x.UserId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(entity => entity.SystemSettings)
               .WithOne(x => x.User)
               .HasForeignKey(x => x.UserId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(entity => entity.MeetingRequests)
               .WithOne(x => x.Staff)
               .HasForeignKey(x => x.StaffId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(entity => entity.SystemSettingLogs)
               .WithOne(x => x.User)
               .HasForeignKey(x => x.UserId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(entity => entity.AdministrativeLogs)
               .WithOne(x => x.User)
               .HasForeignKey(x => x.UserId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(entity => entity.StudentEnrollments)
               .WithOne(x => x.ApprovedByUser)
               .HasForeignKey(x => x.ApprovedByUserId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}

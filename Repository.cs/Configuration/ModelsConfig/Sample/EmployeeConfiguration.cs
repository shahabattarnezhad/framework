using Entities.Models.Sample;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.Configuration.ModelsConfig.Sample;

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.HasKey(entity => entity.Id);

        builder.Property(entity => entity.FullName)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(entity => entity.Position)
               .IsRequired()
               .HasMaxLength(50);

        builder.Property(entity => entity.Age)
               .IsRequired();
    }
}

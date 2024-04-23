using Entities.Models.Sample;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.Configuration.ModelsConfig.Sample;

public class CompanyConfiguration : IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        builder.HasKey(entity => entity.Id);

        builder.HasIndex(entity => entity.Name)
               .IsUnique();

        builder.Property(entity => entity.Name)
               .IsRequired()
               .HasMaxLength(50);

        builder.Property(entity => entity.Address)
               .HasMaxLength(250);

        builder.HasMany(entity => entity.Employees)
               .WithOne(x => x.Company)
               .HasForeignKey(x => x.CompanyId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}

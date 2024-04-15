using Entities.Models.Sample;
using Microsoft.EntityFrameworkCore;
using Repository.Configuration.DataSeeding;
using Repository.Configuration.ModelsConfig.Sample;

namespace Repository.Data;

public class RepositoryContext : DbContext
{
    public RepositoryContext(DbContextOptions options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new CompanyConfiguration());
        modelBuilder.ApplyConfiguration(new EmployeeConfiguration());

        modelBuilder.ApplyConfiguration(new CompanySeeding());
        modelBuilder.ApplyConfiguration(new EmployeeSeeding());
    }

    public DbSet<Company>? Companies { get; set; }
    public DbSet<Employee>? Employees { get; set; }
}

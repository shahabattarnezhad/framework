using Entities.Models.Authentication;
using Entities.Models.Sample;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Repository.Configuration.DataSeeding.Authentication;
using Repository.Configuration.DataSeeding.Sample;
using Repository.Configuration.ModelsConfig.Authentication;
using Repository.Configuration.ModelsConfig.Sample;

namespace Repository.Data;

public class RepositoryContext : IdentityDbContext<User>
{
    public RepositoryContext(DbContextOptions options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new CompanyConfiguration());
        modelBuilder.ApplyConfiguration(new EmployeeConfiguration());

        modelBuilder.ApplyConfiguration(new RoleSeeding());
        modelBuilder.ApplyConfiguration(new CompanySeeding());
        modelBuilder.ApplyConfiguration(new EmployeeSeeding());
    }

    public DbSet<Company>? Companies { get; set; }
    public DbSet<Employee>? Employees { get; set; }
}

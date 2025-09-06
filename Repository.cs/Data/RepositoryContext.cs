using Entities.Models.Authentication;
using Entities.Models.Base;
using Entities.Models.Sample;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Repository.Configuration.DataSeeding.Authentication;
using Repository.Configuration.DataSeeding.Sample;
using Repository.Configuration.ModelsConfig.Authentication;
using Repository.Configuration.ModelsConfig.Sample;

namespace Repository.Data;

public class RepositoryContext : IdentityDbContext<
    User,
    Role,
    string,
    UserClaim,
    UserRole,
    IdentityUserLogin<string>,
    IdentityRoleClaim<string>,
    IdentityUserToken<string>>
{
    private readonly IHttpContextAccessor? _httpContextAccessor;

    public RepositoryContext(DbContextOptions<RepositoryContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public RepositoryContext(DbContextOptions<RepositoryContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuration sample:
        modelBuilder.ApplyConfiguration(new CompanyConfiguration());
        modelBuilder.ApplyConfiguration(new EmployeeConfiguration());

        // Authentication
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new PermissionConfiguration());
        modelBuilder.ApplyConfiguration(new RoleConfiguration());
        modelBuilder.ApplyConfiguration(new RolePermissionConfiguration());
        modelBuilder.ApplyConfiguration(new UserProfileConfiguration());
        modelBuilder.ApplyConfiguration(new UserRoleConfiguration());

        modelBuilder.ApplyConfiguration(new UserSeeding());
        modelBuilder.ApplyConfiguration(new RoleSeeding());
        modelBuilder.ApplyConfiguration(new UserRoleSeeding());
        modelBuilder.ApplyConfiguration(new PermissionSeeding());

        // Seeding sample:
        modelBuilder.ApplyConfiguration(new CompanySeeding());
        modelBuilder.ApplyConfiguration(new EmployeeSeeding());
    }

    // Sample:
    public DbSet<Company>? Companies { get; set; }
    public DbSet<Employee>? Employees { get; set; }

    // Authentication:
    public DbSet<Permission>? Permissions { get; set; }
    public DbSet<RolePermission>? RolePermissions { get; set; }
    public DbSet<UserProfile>? UserProfiles { get; set; }
    public DbSet<UserProfileImage>? UserProfileImages { get; set; }


    // Audit:
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        var entries = ChangeTracker.Entries<BaseEntity<Guid>>();

        foreach (var entry in entries)
        {
            var userId = _httpContextAccessor?.HttpContext?.User?.Identity?.Name;

            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = DateTime.UtcNow;
                entry.Entity.CreatedBy = userId;
                entry.Entity.IsActive = true;
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedAt = DateTime.UtcNow;
                entry.Entity.UpdatedBy = userId;
                entry.Entity.IsEdited = true;
            }
            else if (entry.State == EntityState.Deleted)
            {
                entry.Entity.DeletedAt = DateTime.UtcNow;
                entry.Entity.DeletedBy = userId;
                entry.Entity.IsDeleted = true;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}

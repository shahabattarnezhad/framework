using Entities.Models.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Repository.Configuration.DataSeeding.Constants;
using Repository.Data;
using Shared.Constants;

namespace Repository.Configuration.DataSeeding.Authentication;

public static class PermissionSeeder
{
    public static async Task SeedAsync(RepositoryContext context, CancellationToken cancellationToken = default)
    {
        foreach (var def in PermissionConstants.AllPermissions)
        {
            var exists = await context.Permissions
                .AnyAsync(p => p.Id == def.Id, cancellationToken);

            if (!exists)
            {
                await context.Permissions.AddAsync(new Permission
                {
                    Id = def.Id,
                    Name = def.Name,
                    DisplayName = def.DisplayName
                }, cancellationToken);
            }
            else
            {
                var existing = await context.Permissions.FirstAsync(p => p.Id == def.Id, cancellationToken);
                existing.Name = def.Name;
                existing.DisplayName = def.DisplayName;
            }
        }

        await context.SaveChangesAsync(cancellationToken);
    }
}

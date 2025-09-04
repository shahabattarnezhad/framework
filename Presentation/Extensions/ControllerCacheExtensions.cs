using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Service.Contracts.Base;

namespace Presentation.Extensions;

public static class ControllerCacheExtensions
{
    /// <summary>
    /// Invalidates only the list cache for a given entity (inside controller).
    /// </summary>
    public static async Task InvalidateEntityCacheAsync
        (this ControllerBase controller, string entityName, CancellationToken cancellationToken = default)
    {
        var cacheService =
            controller.HttpContext.RequestServices.GetRequiredService<ICacheInvalidationService>();

        await cacheService.InvalidateEntityCacheAsync(entityName, cancellationToken);
    }

    /// <summary>
    /// Invalidates both list + detail cache for a given entity (inside controller).
    /// </summary>
    public static async Task InvalidateEntityCacheAsync<TEntityId>(this ControllerBase controller, string entityName,
        TEntityId? entityId = default, CancellationToken cancellationToken = default)
    {
        var cacheService = controller.HttpContext.RequestServices.GetRequiredService<ICacheInvalidationService>();
        await cacheService.InvalidateEntityCacheAsync(entityName, entityId, cancellationToken);
    }

    /// <summary>
    /// Invalidates cache for multiple entities (list cache only).
    /// </summary>
    public static async Task InvalidateEntitiesCacheAsync
        (this ControllerBase controller, IEnumerable<string> entityNames, CancellationToken cancellationToken = default)
    {
        var cacheService = controller.HttpContext.RequestServices.GetRequiredService<ICacheInvalidationService>();
        await cacheService.InvalidateEntitiesCacheAsync(entityNames, cancellationToken);
    }
}

using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Extensions.Logging;
using Service.Contracts.Base;

namespace Service.Base;

public class CacheInvalidationService : ICacheInvalidationService
{
    private readonly IOutputCacheStore _cacheStore;
    private readonly ILogger<CacheInvalidationService> _logger;

    public CacheInvalidationService(IOutputCacheStore cacheStore, ILogger<CacheInvalidationService> logger)
    {
        _cacheStore = cacheStore ?? throw new ArgumentNullException(nameof(cacheStore));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task InvalidateEntityCacheAsync<TEntityId>
        (string entityName, TEntityId? entityId = default, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(entityName))
            throw new ArgumentException("Entity name cannot be null or empty.", nameof(entityName));

        // Always invalidate the list cache
        var listTag = $"{entityName}ListCache";

        await _cacheStore.EvictByTagAsync(listTag, cancellationToken);

        // If an entityId is provided, invalidate the detail cache as well
        if (entityId is not null)
        {
            var detailTag = $"{entityName}DetailCache-{entityId}";

            await _cacheStore.EvictByTagAsync(detailTag, cancellationToken);
        }
    }
}

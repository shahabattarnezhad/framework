using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Extensions.Caching.Distributed;
using Service.Contracts.Base;
using Service.Helpers;
using System.Text.Json;

namespace Service.Base;

public class CacheRefresherService<TEntity, TEntityId> : ICacheRefresherService<TEntity, TEntityId>
{
    private readonly IOutputCacheStore _cacheStore;

    public CacheRefresherService(IOutputCacheStore cacheStore)
    {
        _cacheStore = cacheStore ?? throw new ArgumentNullException(nameof(cacheStore));
    }

    public async Task RefreshListCacheAsync(
        string entityName,
        Func<CancellationToken, Task<IEnumerable<TEntity>>> dataFetcher,
        CancellationToken cancellationToken = default)
    {
        var listTag = CacheTagFormatter.ListTag(entityName);

        // Invalidate old cache
        await _cacheStore.EvictByTagAsync(listTag, cancellationToken);

        // Fetch fresh data
        var data = await dataFetcher(cancellationToken);

        // Store back to cache (fake example: you need to serialize and define your caching logic here)
        await _cacheStore.SetAsync(
            key: listTag,
            value: JsonSerializer.SerializeToUtf8Bytes(data),
            tags: new[] { listTag },
            validFor: TimeSpan.FromMinutes(5),
            cancellationToken: cancellationToken);
    }

    public async Task RefreshDetailCacheAsync(
        string entityName,
        TEntityId entityId,
        Func<CancellationToken, Task<TEntity>> dataFetcher,
        CancellationToken cancellationToken = default)
    {
        var detailTag = CacheTagFormatter.DetailTag(entityName, entityId);

        // Invalidate old cache
        await _cacheStore.EvictByTagAsync(detailTag, cancellationToken);

        // Fetch fresh data
        var data = await dataFetcher(cancellationToken);

        // Store back to cache
        await _cacheStore.SetAsync(
            key: detailTag,
            value: JsonSerializer.SerializeToUtf8Bytes(data),
            tags: new[] { detailTag },
            validFor: TimeSpan.FromMinutes(5),
            cancellationToken: cancellationToken);
    }
}

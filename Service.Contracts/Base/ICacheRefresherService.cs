namespace Service.Contracts.Base;

public interface ICacheRefresherService<TEntity, TEntityId>
{
    Task RefreshListCacheAsync(
        string entityName,
        Func<CancellationToken, Task<IEnumerable<TEntity>>> dataFetcher,
        CancellationToken cancellationToken = default);

    Task RefreshDetailCacheAsync(
        string entityName,
        TEntityId entityId,
        Func<CancellationToken, Task<TEntity>> dataFetcher,
        CancellationToken cancellationToken = default);
}

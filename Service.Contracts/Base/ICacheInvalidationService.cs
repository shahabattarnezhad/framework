namespace Service.Contracts.Base;

public interface ICacheInvalidationService
{
    /// <summary>
    /// Invalidates cache entries for a given entity type.
    /// </summary>
    /// <typeparam name="TEntityId">Type of the entity's identifier (e.g., Guid, int, string)</typeparam>
    /// <param name="entityName">Logical name of the entity (e.g., "Teacher", "Student")</param>
    /// <param name="entityId">
    /// Optional entity identifier. 
    /// If null → only the list cache is invalidated.
    /// If provided → both list and detail cache for this entity will be invalidated.
    /// </param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task InvalidateEntityCacheAsync<TEntityId>
        (string entityName, TEntityId? entityId = default, CancellationToken cancellationToken = default);
}

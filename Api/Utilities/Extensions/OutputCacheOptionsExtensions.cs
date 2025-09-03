using Microsoft.AspNetCore.OutputCaching;

namespace Api.Utilities.Extensions;

public static class OutputCacheOptionsExtensions
{
    /// <summary>
    /// Adds caching policies for an entity (list + detail).
    /// </summary>
    /// <typeparam name="TEntityId">Type of the entity's identifier (e.g., Guid, int, string)</typeparam>
    /// <param name="options">Output cache options builder</param>
    /// <param name="entityName">Logical name of the entity (e.g., "Teacher", "Student")</param>
    /// <param name="listDuration">Expiration time for the list cache</param>
    /// <param name="detailDuration">Expiration time for the detail cache</param>
    public static void AddEntityCachePolicies<TEntityId>(
        this OutputCacheOptions options,
        string entityName,
        TimeSpan listDuration,
        TimeSpan detailDuration)
    {
        if (string.IsNullOrWhiteSpace(entityName))
            throw new ArgumentException("Entity name cannot be null or empty.", nameof(entityName));

        options.AddPolicy($"{entityName}ListCachePolicy", policy =>
        {
            policy.Expire(listDuration)
                  .Tag($"{entityName}ListCache");
        });


        options.AddPolicy($"{entityName}DetailCachePolicy", policy =>
        {
            policy.Expire(detailDuration)
                  .SetVaryByRouteValue("id")
            .Tag($"{entityName}DetailCache-{{id}}");
        });
    }
}

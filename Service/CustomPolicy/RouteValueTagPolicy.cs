using System.Globalization;
using Microsoft.AspNetCore.OutputCaching;
using Service.Helpers;

namespace Service.CustomPolicy;

public sealed class RouteValueTagPolicy : IOutputCachePolicy
{
    private readonly string _entityName;
    private readonly string _routeParam;

    public RouteValueTagPolicy(string entityName, string routeParam = "id")
    {
        _entityName = entityName ?? throw new ArgumentNullException(nameof(entityName));
        _routeParam = routeParam ?? throw new ArgumentNullException(nameof(routeParam));
    }

    public ValueTask CacheRequestAsync(OutputCacheContext context, CancellationToken cancellation)
    {
        TryAddTag(context);
        return default;
    }

    public ValueTask ServeFromCacheAsync(OutputCacheContext context) => default;

    public ValueTask ServeFromCacheAsync(OutputCacheContext context, CancellationToken cancellation)
    {
        throw new NotImplementedException();
    }

    public ValueTask ServeResponseAsync(OutputCacheContext context, CancellationToken cancellation) => default;

    private void TryAddTag(OutputCacheContext context)
    {
        var routeValues = context.HttpContext.Request.RouteValues;
        if (routeValues.TryGetValue(_routeParam, out var idObj) && idObj is not null)
        {
            var idStr = CacheTagIdNormalizer.Normalize(idObj);
            context.Tags.Add($"{_entityName}DetailCache-{idStr}");
        }
    }
}

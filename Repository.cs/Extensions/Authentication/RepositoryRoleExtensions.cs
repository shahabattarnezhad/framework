using Entities.Models.Authentication;
using Repository.Extensions.Utility;
using System.Linq.Dynamic.Core;

namespace Repository.Extensions.Authentication;

public static class RepositoryRoleExtensions
{
    public static IQueryable<Role> Search(this IQueryable<Role> entities, string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return entities;

        var lowerCaseTerm = searchTerm.Trim().ToLower();

        return entities.Where(entity => entity.Name!.ToLower().Contains(lowerCaseTerm));
    }


    public static IQueryable<Role> Sort(this IQueryable<Role> entities, string orderByQueryString)
    {
        if (string.IsNullOrWhiteSpace(orderByQueryString))
            return entities.OrderBy(entity => entity.Name);

        var orderQuery = OrderQueryBuilder.CreateOrderQuery<Role>(orderByQueryString);

        if (string.IsNullOrWhiteSpace(orderQuery))
            return entities.OrderBy(entity => entity.Name);

        return entities.OrderBy(orderQuery);
    }
}

using Entities.Models.Authentication;
using Repository.Extensions.Utility;
using System.Linq.Dynamic.Core;

namespace Repository.Extensions.Authentication;

public static class RepositoryUserExtensions
{
    public static IQueryable<User> Search(this IQueryable<User> entities, string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return entities;

        var lowerCaseTerm = searchTerm.Trim().ToLower();

        return entities.Where(entity => entity.UserName!.ToLower().Contains(lowerCaseTerm) ||
                                                entity.FirstName!.ToLower().Contains(lowerCaseTerm) ||
                                                entity.LastName!.ToLower().Contains(lowerCaseTerm));
    }


    public static IQueryable<User> Sort(this IQueryable<User> entities, string orderByQueryString)
    {
        if (string.IsNullOrWhiteSpace(orderByQueryString))
            return entities.OrderBy(entity => entity.UserName);

        var orderQuery = OrderQueryBuilder.CreateOrderQuery<User>(orderByQueryString);

        if (string.IsNullOrWhiteSpace(orderQuery))
            return entities.OrderBy(entity => entity.UserName);

        return entities.OrderBy(orderQuery);
    }
}

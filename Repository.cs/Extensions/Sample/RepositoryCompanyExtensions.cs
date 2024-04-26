using Entities.Models.Sample;
using Repository.Extensions.Utility;
using System.Linq.Dynamic.Core;

namespace Repository.Extensions.Sample;

public static class RepositoryCompanyExtensions
{
    public static IQueryable<Company> Search(this IQueryable<Company> entities,
                                                   string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return entities;

        var lowerCaseTerm = searchTerm.Trim()
                                             .ToLower();

        return entities.Where(entity => entity.Name!
                                                     .ToLower()
                                                     .Contains(lowerCaseTerm));
    }


    public static IQueryable<Company> Sort(this IQueryable<Company> entities,
                                                 string orderByQueryString)
    {
        if (string.IsNullOrWhiteSpace(orderByQueryString))
            return entities.OrderBy(entity => entity.Name);

        var orderQuery = 
            OrderQueryBuilder.CreateOrderQuery<Company>(orderByQueryString);        

        if (string.IsNullOrWhiteSpace(orderQuery)) 
            return entities.OrderBy(entity => entity.Name);

        return entities.OrderBy(orderQuery);
    }
}

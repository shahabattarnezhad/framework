using Entities.Models.Sample;
using Repository.Extensions.Utility;
using System.Linq.Dynamic.Core;

namespace Repository.Extensions.Sample;

public static class RepositoryEmployeeExtensions
{
    public static IQueryable<Employee> FilterEmployees
        (this IQueryable<Employee> employees, uint minAge, uint maxAge) =>
        employees.Where(e => (e.Age >= minAge && e.Age <= maxAge));


    public static IQueryable<Employee> Search(this IQueryable<Employee> employees,
                                                   string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return employees;

        var lowerCaseTerm = searchTerm.Trim()
                                             .ToLower();

        return employees.Where(e => e.FullName!
                                                     .ToLower()
                                                     .Contains(lowerCaseTerm));
    }


    public static IQueryable<Employee> Sort(this IQueryable<Employee> employees,
                                                 string orderByQueryString)
    {
        if (string.IsNullOrWhiteSpace(orderByQueryString))
            return employees.OrderBy(e => e.FullName);

        var orderQuery = 
            OrderQueryBuilder.CreateOrderQuery<Employee>(orderByQueryString);        

        if (string.IsNullOrWhiteSpace(orderQuery)) 
            return employees.OrderBy(e => e.FullName);

        return employees.OrderBy(orderQuery);
    }
}

using Entities.Models.Sample;
using System.Reflection;
using System.Text;

namespace Repository.Extensions.Utility;

public static class OrderQueryBuilder
{
    public static string CreateOrderQuery<T>(string orderByQueryString)
    {
        var orderParams = orderByQueryString.Trim().Split(',');

        var orderQueryBuilder = new StringBuilder();

        foreach (var param in orderParams)
        {
            if (string.IsNullOrWhiteSpace(param))
                continue;

            var propertyFromQueryName = param.Split(" ")[0];

            var direction = param.EndsWith(" desc") ? "descending" : "ascending";

            var parts = propertyFromQueryName.Split('.');
            Type type = typeof(T);
            bool valid = true;
            string propertyPath = "";

            foreach (var part in parts)
            {
                var propertyInfo = type.GetProperty(part, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (propertyInfo == null)
                {
                    valid = false;
                    break;
                }

                type = propertyInfo.PropertyType;

                if (string.IsNullOrEmpty(propertyPath))
                    propertyPath = propertyInfo.Name;
                else
                    propertyPath += "." + propertyInfo.Name;
            }

            if (!valid)
                continue;

            orderQueryBuilder.Append($"{propertyPath} {direction}, ");
        }

        var orderQuery = orderQueryBuilder.ToString().TrimEnd(',', ' ');

        return orderQuery;
    }
}

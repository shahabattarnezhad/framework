using Contracts.DataShaping;
using Service.DataShaping;
using Shared.DTOs.Sample.Employee;

namespace Api.Utilities.Extensions.Sample;

public static class DataShapingRegisteration
{
    public static void ConfigureEmployeeDataShaper(this IServiceCollection services) =>
        services.AddScoped<IDataShaper<EmployeeDto>, DataShaper<EmployeeDto>>();
}

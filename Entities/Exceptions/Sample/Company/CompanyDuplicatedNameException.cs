using Entities.Exceptions.Base;

namespace Entities.Exceptions.Sample.Company;

public sealed class CompanyDuplicatedNameException : BadRequestException
{
    public CompanyDuplicatedNameException() 
        : base("The entered name for this company is exists. Try another name.") { }
}

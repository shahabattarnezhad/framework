using Entities.Exceptions.Base;

namespace Entities.Exceptions.Sample.Company;

public sealed class CompanyCollectionBadRequest : BadRequestException
{
    public CompanyCollectionBadRequest() 
        : base("Company collection sent from a client is null.") { }
}

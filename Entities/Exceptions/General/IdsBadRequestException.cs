using Entities.Exceptions.Base;

namespace Entities.Exceptions.General;

public sealed class IdsBadRequestException : BadRequestException
{
    public IdsBadRequestException() : base("Ids sent in the request are not valid.")
    {
    }
}

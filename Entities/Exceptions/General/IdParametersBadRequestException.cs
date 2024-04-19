using Entities.Exceptions.Base;

namespace Entities.Exceptions.General;

public sealed class IdParametersBadRequestException : BadRequestException
{
    public IdParametersBadRequestException() : base("Parameter ids is null") { }
}

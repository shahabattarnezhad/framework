using Entities.Exceptions.Base;

namespace Entities.Exceptions.General;

public sealed class MaxAgeRangeBadRequestException : BadRequestException
{
    public MaxAgeRangeBadRequestException()
        : base("Max age can't be less than min age.") { }
}

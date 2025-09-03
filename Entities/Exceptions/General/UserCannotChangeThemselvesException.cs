using Entities.Exceptions.Base;

namespace Entities.Exceptions.General;

public sealed class UserCannotChangeThemselvesException : BadRequestException
{
    public UserCannotChangeThemselvesException()
       : base("Current user cannot disable themselves.")
    {
    }
}

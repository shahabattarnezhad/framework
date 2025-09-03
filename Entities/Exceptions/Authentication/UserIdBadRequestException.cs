using Entities.Exceptions.Base;

namespace Entities.Exceptions.Authentication;

public sealed class UserIdBadRequestException : BadRequestException
{
    public UserIdBadRequestException(string userId)
        : base($"The user with Id: {userId} doesn't exist in the database.")
    {
    }
}

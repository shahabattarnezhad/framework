using Entities.Exceptions.Base;

namespace Entities.Exceptions.Authentication;

public sealed class UserNotFoundByUserNameException : NotFoundException
{
    public UserNotFoundByUserNameException(string userName)
        : base($"The user with username: {userName} doesn't exist in the database.")
    {
    }
}

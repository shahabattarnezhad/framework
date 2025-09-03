using Entities.Exceptions.Base;

namespace Entities.Exceptions.Authentication;

public sealed class UserNotFoundByIdException : NotFoundException
{
    public UserNotFoundByIdException(string userId)
        : base($"The user with id: {userId} doesn't exist in the database.")
    {
    }
}

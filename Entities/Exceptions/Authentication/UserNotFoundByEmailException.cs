using Entities.Exceptions.Base;

namespace Entities.Exceptions.Authentication;

public sealed class UserNotFoundByEmailException : NotFoundException
{
    public UserNotFoundByEmailException(string email)
        : base($"The user with email: {email} doesn't exist in the database.")
    {
    }
}

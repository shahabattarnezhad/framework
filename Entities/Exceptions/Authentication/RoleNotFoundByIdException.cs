using Entities.Exceptions.Base;

namespace Entities.Exceptions.Authentication;

public sealed class RoleNotFoundByIdException : NotFoundException
{
    public RoleNotFoundByIdException(string roleId)
        : base($"The role with id: {roleId} doesn't exist in the database.")
    {
    }
}

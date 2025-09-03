using Entities.Exceptions.Base;

namespace Entities.Exceptions.Authentication;

public sealed class RoleNotFoundByNameException : NotFoundException
{
    public RoleNotFoundByNameException(string roleName)
        : base($"The role with rolename: {roleName} doesn't exist in the database.")
    {
    }
}

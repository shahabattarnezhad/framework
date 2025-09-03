using Entities.Exceptions.Base;

namespace Entities.Exceptions.Authentication;

public sealed class RegistrationForbiddenException : ForbiddenException
{
    public RegistrationForbiddenException() : base("Registration is not allowed.") { }
}

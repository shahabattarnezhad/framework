namespace Entities.Exceptions.Base;

public abstract class ForbiddenException : Exception
{
    protected ForbiddenException(string message) : base(message) { }
}

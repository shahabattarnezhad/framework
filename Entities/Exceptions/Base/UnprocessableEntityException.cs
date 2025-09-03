namespace Entities.Exceptions.Base;

public sealed class UnprocessableEntityException : Exception
{
    public object Errors { get; }

    public UnprocessableEntityException(object errors) : base("One or more validation errors occurred.")
    {
        Errors = errors;
    }
}

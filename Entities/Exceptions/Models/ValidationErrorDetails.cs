namespace Entities.Exceptions.Models;

public sealed class ValidationErrorDetails
{
    public string Message { get; init; } = "One or more validation errors occurred.";

    public IDictionary<string, string[]> Errors { get; init; } = new Dictionary<string, string[]>();
}

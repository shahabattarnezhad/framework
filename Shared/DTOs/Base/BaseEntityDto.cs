namespace Shared.DTOs.Base;

public record BaseEntityDto<T> where T : struct
{
    public T Id { get; init; }
}

namespace Shared.DTOs.Base;

public class BaseEntityDto<T> where T : struct
{
    public T Id { get; set; }
}

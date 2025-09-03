namespace Shared.DTOs.Base;

public record BaseEntityDto<T> where T : struct
{
    public T Id { get; init; }

    public DateTime CreatedAt { get; init; }

    public DateTime? UpdatedAt { get; init; }

    public DateTime? DeletedAt { get; init; }

    public string? CreatedBy { get; init; }

    public string? UpdatedBy { get; init; }

    public string? DeletedBy { get; init; }

    public bool IsActive { get; init; }

    public bool IsDeleted { get; init; }

    public bool IsEdited { get; init; }

    public string? Description { get; init; }
}


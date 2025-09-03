namespace Entities.Models.Base;

public abstract class BaseEntity<T> : IEntity<T> where T : struct
{
    public T Id { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public string? CreatedBy { get; set; }

    public string? UpdatedBy { get; set; }

    public string? DeletedBy { get; set; }

    public bool IsActive { get; set; }

    public bool IsDeleted { get; set; }

    public bool IsEdited { get; set; }

    public string? Description { get; set; }
}

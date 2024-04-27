namespace Entities.Models.Base;

public class ShapedEntity : BaseEntity<Guid>
{
    public ShapedEntity()
    {
        Entity = new Entity();
    }

    public Entity Entity { get; set; }
}

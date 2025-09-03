namespace Entities.Models.Base;

public class ShapedEntity<TId>
{
    public ShapedEntity()
    {
        Entity = new Entity();
    }

    public TId Id { get; set; }
    public Entity Entity { get; set; }
}

using Entities.Exceptions.Base;

namespace Entities.Exceptions.General;

public sealed class EntitiesNotFoundException : NotFoundException
{
    public EntitiesNotFoundException() : base("No entities found matching the specified criteria.")
    {
    }
}

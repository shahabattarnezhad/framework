using Entities.Exceptions.Base;

namespace Entities.Exceptions.General;

public sealed class EntityCollectionBadRequest : BadRequestException
{
    public EntityCollectionBadRequest() 
        : base("Entity collection sent from a client is null.") { }
}

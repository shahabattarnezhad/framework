using Entities.Exceptions.Base;

namespace Entities.Exceptions.General;

public sealed class EntityIdBadRequestException : BadRequestException
{
    public EntityIdBadRequestException(Guid entityId)
        : base($"The entity with id: {entityId} doesn't exist in the database.")
    {
    }
}

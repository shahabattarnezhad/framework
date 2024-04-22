using Entities.Exceptions.Base;

namespace Entities.Exceptions.General;

public sealed class EntityCannotBeDeletedException : BadRequestException
{
    public EntityCannotBeDeletedException()
       : base("The requested entity cannot be deleted because it has children.")
    {
    }
}

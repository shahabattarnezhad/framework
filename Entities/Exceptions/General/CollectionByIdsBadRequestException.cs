using Entities.Exceptions.Base;

namespace Entities.Exceptions.General;

public sealed class CollectionByIdsBadRequestException : BadRequestException
{
    public CollectionByIdsBadRequestException() 
        :base("Collection count mismatch comparing to ids.") { }
}

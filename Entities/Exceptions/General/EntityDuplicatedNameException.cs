using Entities.Exceptions.Base;

namespace Entities.Exceptions.General;

public sealed class EntityDuplicatedNameException : BadRequestException
{
    public EntityDuplicatedNameException() 
        : base("The entered name for this entity is exists. Try another name.") { }
}

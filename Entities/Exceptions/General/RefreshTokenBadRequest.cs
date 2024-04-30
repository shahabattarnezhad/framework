using Entities.Exceptions.Base;

namespace Entities.Exceptions.General;

public sealed class RefreshTokenBadRequest : BadRequestException
{
    public RefreshTokenBadRequest()
        : base("Invalid client request. The tokenDto has some invalid values.") { }
}

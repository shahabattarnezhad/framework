using Shared.DTOs.Base;

namespace Shared.DTOs.Authentication;

public readonly record struct PermissionDto(Guid Id, string Name, string DisplayName);

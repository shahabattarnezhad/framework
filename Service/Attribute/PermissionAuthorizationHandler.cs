using Contracts.Base;
using Entities.Models.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Service.Attribute;

public class PermissionAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement>
{
    private readonly UserManager<User> _userManager;
    private readonly IRepositoryManager _repository;

    public PermissionAuthorizationHandler(UserManager<User> userManager, IRepositoryManager repository)
    {
        _userManager = userManager;
        _repository = repository;
    }

    protected override async Task HandleRequirementAsync
        (AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement)
    {
        var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
            return;

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return;

        var userRoles = await _userManager.GetRolesAsync(user);
        if (!userRoles.Any())
            return;


        var permissions = 
            await _repository.RolePermission.GetPermissionsByRoleNamesAsync(userRoles, false);

        if (permissions.Any(p => p.Name == requirement.Name))
        {
            context.Succeed(requirement);
        }
    }
}


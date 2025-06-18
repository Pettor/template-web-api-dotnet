using Backend.Application.Identity.Users;
using Backend.Shared.Authorization;
using Microsoft.AspNetCore.Authorization;

namespace Backend.Infrastructure.Auth.Permissions;

internal class PermissionAuthorizationHandler(IUserService userService)
    : AuthorizationHandler<PermissionRequirement>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionRequirement requirement
    )
    {
        if (
            context.User?.GetUserId() is { } userId
            && await userService.HasPermissionAsync(userId, requirement.Permission)
        )
        {
            context.Succeed(requirement);
        }
    }
}

using Backend.Application.Common.Caching;
using Backend.Application.Common.Exceptions;
using Backend.Shared.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Identity;

internal partial class UserService
{
    public async Task<List<string>> GetPermissionsAsync(
        string userId,
        CancellationToken cancellationToken
    )
    {
        var user = await userManager.FindByIdAsync(userId);

        _ = user ?? throw new NotFoundException(localizer["User Not Found."]);

        var userRoles = await userManager.GetRolesAsync(user);
        var permissions = new List<string>();
        foreach (
            var role in await roleManager
                .Roles.Where(r => userRoles.Contains(r.Name))
                .ToListAsync(cancellationToken)
        )
        {
            permissions.AddRange(
                await db
                    .RoleClaims.Where(rc =>
                        rc.RoleId == role.Id && rc.ClaimType == ApiClaims.Permission
                    )
                    .Select(rc => rc.ClaimValue ?? "N/A")
                    .ToListAsync(cancellationToken)
            );
        }

        return permissions.Distinct().ToList();
    }

    public async Task<bool> HasPermissionAsync(
        string userId,
        string permission,
        CancellationToken cancellationToken
    )
    {
        var permissions = await cache.GetOrSetAsync(
            cacheKeys.GetCacheKey(ApiClaims.Permission, userId),
            () => GetPermissionsAsync(userId, cancellationToken),
            cancellationToken: cancellationToken
        );

        return permissions?.Contains(permission) ?? false;
    }

    public Task InvalidatePermissionCacheAsync(
        string userId,
        CancellationToken cancellationToken
    ) => cache.RemoveAsync(cacheKeys.GetCacheKey(ApiClaims.Permission, userId), cancellationToken);
}

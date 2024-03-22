using Microsoft.AspNetCore.Authorization;

namespace Backend.Infrastructure.Auth.Permissions;

internal class PermissionRequirement(string permission) : IAuthorizationRequirement
{
    public string Permission { get; private set; } = permission;
}

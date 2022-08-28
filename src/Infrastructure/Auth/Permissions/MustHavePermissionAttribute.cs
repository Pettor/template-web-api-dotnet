using Microsoft.AspNetCore.Authorization;
using WebApiTemplate.Shared.Authorization;

namespace WebApiTemplate.Infrastructure.Auth.Permissions;

public class MustHavePermissionAttribute : AuthorizeAttribute
{
    public MustHavePermissionAttribute(string action, string resource) =>
        Policy = FshPermission.NameFor(action, resource);
}
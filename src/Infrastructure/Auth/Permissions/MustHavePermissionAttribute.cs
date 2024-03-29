﻿using Backend.Shared.Authorization;
using Microsoft.AspNetCore.Authorization;

namespace Backend.Infrastructure.Auth.Permissions;

public class MustHavePermissionAttribute : AuthorizeAttribute
{
    public MustHavePermissionAttribute(string action, string resource) =>
        Policy = ApiPermission.NameFor(action, resource);
}

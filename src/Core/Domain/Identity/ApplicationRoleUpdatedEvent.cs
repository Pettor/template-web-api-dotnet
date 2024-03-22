namespace Backend.Domain.Identity;

public class ApplicationRoleUpdatedEvent(string roleId, string roleName, bool permissionsUpdated = false) : ApplicationRoleEvent(roleId, roleName)
{
    public bool PermissionsUpdated { get; set; } = permissionsUpdated;
}

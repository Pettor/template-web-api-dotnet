namespace Backend.Domain.Identity;

public class ApplicationRoleUpdatedEvent : ApplicationRoleEvent
{
    public bool PermissionsUpdated { get; set; }

    public ApplicationRoleUpdatedEvent(string roleId, string roleName, bool permissionsUpdated = false)
        : base(roleId, roleName) =>
        PermissionsUpdated = permissionsUpdated;
}
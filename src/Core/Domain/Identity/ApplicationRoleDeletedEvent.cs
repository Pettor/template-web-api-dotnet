namespace Backend.Domain.Identity;

public class ApplicationRoleDeletedEvent : ApplicationRoleEvent
{
    public bool PermissionsUpdated { get; set; }

    public ApplicationRoleDeletedEvent(string roleId, string roleName)
        : base(roleId, roleName)
    {
    }
}
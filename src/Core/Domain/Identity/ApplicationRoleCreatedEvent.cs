namespace Backend.Domain.Identity;

public class ApplicationRoleCreatedEvent : ApplicationRoleEvent
{
    public ApplicationRoleCreatedEvent(string roleId, string roleName)
        : base(roleId, roleName)
    {
    }
}

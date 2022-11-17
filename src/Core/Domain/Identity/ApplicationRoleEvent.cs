using Backend.Domain.Common.Contracts;

namespace Backend.Domain.Identity;

public abstract class ApplicationRoleEvent : DomainEvent
{
    public string RoleId { get; set; } = default!;
    public string RoleName { get; set; } = default!;
    protected ApplicationRoleEvent(string roleId, string roleName) =>
        (RoleId, RoleName) = (roleId, roleName);
}

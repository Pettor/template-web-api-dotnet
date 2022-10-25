using WebApiTemplate.Domain.Common.Contracts;

namespace WebApiTemplate.Domain.Identity;

public abstract class ApplicationRoleEvent : DomainEvent
{
    public string RoleId { get; set; } = default!;
    public string RoleName { get; set; } = default!;
    protected ApplicationRoleEvent(string roleId, string roleName) =>
        (RoleId, RoleName) = (roleId, roleName);
}
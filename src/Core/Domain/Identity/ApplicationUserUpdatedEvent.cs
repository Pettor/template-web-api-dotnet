namespace Backend.Domain.Identity;

public class ApplicationUserUpdatedEvent : ApplicationUserEvent
{
    public bool RolesUpdated { get; set; }

    public ApplicationUserUpdatedEvent(string userId, bool rolesUpdated = false)
        : base(userId) =>
        RolesUpdated = rolesUpdated;
}

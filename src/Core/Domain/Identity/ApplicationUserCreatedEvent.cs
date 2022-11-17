namespace Backend.Domain.Identity;

public class ApplicationUserCreatedEvent : ApplicationUserEvent
{
    public ApplicationUserCreatedEvent(string userId)
        : base(userId)
    {
    }
}

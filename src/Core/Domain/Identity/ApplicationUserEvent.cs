using Backend.Domain.Common.Contracts;

namespace Backend.Domain.Identity;

public abstract class ApplicationUserEvent : DomainEvent
{
    public string UserId { get; set; } = default!;

    protected ApplicationUserEvent(string userId) => UserId = userId;
}

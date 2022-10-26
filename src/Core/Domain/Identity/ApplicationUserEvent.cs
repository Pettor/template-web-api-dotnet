using WebApiTemplate.Domain.Common.Contracts;

namespace WebApiTemplate.Domain.Identity;

public abstract class ApplicationUserEvent : DomainEvent
{
    public string UserId { get; set; } = default!;

    protected ApplicationUserEvent(string userId) => UserId = userId;
}